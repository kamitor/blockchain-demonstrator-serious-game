using BlockchainDemonstratorApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Player
    {
        [Key] 
        public string Id { get; set; }
        
        [Required] 
        public string Name { get; set; }
        
        public Role Role { get; set; }
        
        public double Profit { get { return Balance - (Factors.InitialCapital + Factors.SetupCost); }}
        
        public int Inventory { get; set; } = 20;
        
        public int Backorder
        {
            get
            {
                if (IncomingOrders.Count == 0) return 0;
                int max = IncomingOrders.Max(o => o.OrderDay);
                return IncomingOrders.Where(o => o.OrderDay != max).Sum(o => o.Volume);
            }
        }
        
        public Order CurrentOrder { get; set; }
        
        private List<Order> _outgoingOrders;
        
        /// <summary>
        /// The orders from the player itself
        /// </summary>
        [ForeignKey("OutgoingOrderForPlayerId")]
        public List<Order> OutgoingOrders
        {
            get { return _outgoingOrders; }
            set { _outgoingOrders = value.OrderBy(o => o.OrderDay).ToList(); }
        }

        private List<Order> _incomingOrders;
        
        /// <summary>
        /// Order sent from your customer
        /// </summary>
        [ForeignKey("IncomingOrderForPlayerId")]
        public List<Order> IncomingOrders 
        {
            get { return _incomingOrders; }
            set { _incomingOrders = value.OrderBy(o => o.OrderDay).ToList(); }
        }

        [ForeignKey("PlayerId")] 
        public List<Payment> Payments { get; set; }
        public double Balance { get; set; }

        [NotMapped]
        public double HoldingCosts
        {
            get
            {
                //running cost= (volume of inventory* holding cost factor)+ (backorder factor* backorder* holding cost)+ (incoming order* holding cost) 
                return (Inventory * Factors.HoldingFactor) +
                       (Factors.HoldingFactor * 2 * Backorder) /*+ (IncomingOrder.Volume * holdingFactor)*/;
            }
        }

        public Player(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            IncomingOrders = new List<Order>();
            OutgoingOrders = new List<Order>();
            Payments = new List<Payment>();
        }

        [JsonConstructor]
        public Player(string name, string playerId)
        {
            Id = playerId;
            Name = name;
            IncomingOrders = new List<Order>();
            OutgoingOrders = new List<Order>();
            Payments = new List<Payment>();
        }

        /**
         * <summary>Gets list of outgoing deliveries</summary>
         * <returns>List of Order objects with available stock</returns>
         * <param name="currentDay">integer that specifies the current day</param>
         */
        //TODO: check if you can get paid here instead of in GetOutGoingPayments()
        public void GetOutgoingDeliveries(int currentDay) //Reworked to new order system
        {
            List<Delivery> outgoingDeliveries = new List<Delivery>();
            for (int i = 0; i < IncomingOrders.Count; i++)
            {
                int pendingVolume = IncomingOrders[i].Volume - IncomingOrders[i].Deliveries.Sum(d => d.Volume);
                if (pendingVolume <= Inventory)
                {
                    Inventory -= pendingVolume;
                    int price;
                    switch (Role.Id)  //TODO: rework partial payment
                    {
                        case "Retailer":
                            price = pendingVolume * Factors.RetailProductPrice;
                            break;
                        case "Manufacturer":
                            price = pendingVolume * Factors.ManuProductPrice;
                            break;
                        case "Processor":
                            price = pendingVolume * Factors.ProcProductPrice;
                            break;
                        case "Farmer":
                            price = pendingVolume * Factors.FarmerProductPrice;
                            break;
                        default:
                            price = 0;
                            break;
                    }

                    Delivery delivery = new Delivery()
                    {
                        Volume = pendingVolume,
                        SendDeliveryDay = currentDay,
                        ArrivalDay = currentDay + Role.LeadTime + new Random().Next(0, 4),
                        Price = price
                    };
                    IncomingOrders[i].Deliveries.Add(delivery);
                    outgoingDeliveries.Add(delivery);

                    IncomingOrders.RemoveAt(i);
                    i--;
                    
                    
                }
                else if(Inventory > 0)
                {
                    int price;
                    switch (Role.Id)  //TODO: rework partial payment
                    {
                        case "Retailer":
                            price = Inventory * Factors.RetailProductPrice;
                            break;
                        case "Manufacturer":
                            price = Inventory * Factors.ManuProductPrice;
                            break;
                        case "Processor":
                            price = Inventory * Factors.ProcProductPrice;
                            break;
                        case "Farmer":
                            price = Inventory * Factors.FarmerProductPrice;
                            break;
                        default:
                            price = 0;
                            break;
                    }
                    Delivery delivery = new Delivery()
                    {
                        Volume = Inventory,
                        SendDeliveryDay = currentDay,
                        ArrivalDay = currentDay + Role.LeadTime,
                        Price = price
                    };
                    IncomingOrders[i].Deliveries.Add(delivery);
                    outgoingDeliveries.Add(delivery);

                    Inventory = 0;
                }
            }

            SetTransportCosts(currentDay, outgoingDeliveries);
        }

        /**
         * <summary>Adds the volume of incoming deliveries to inventory where arrival day is in the current round</summary>
         * <remarks>Also adds a payment object to the Payments list for each received delivery</remarks>
         * <param name="currentDay">integer that specifies the current day</param>
         */
        //TODO: test for payment bugs
        public void ProcessDeliveries(int currentDay) //Reworked to new order system

        {
            foreach (Order order in OutgoingOrders)
            {
                foreach (Delivery delivery in order.Deliveries)
                {
                    if(Math.Floor(delivery.ArrivalDay) <= currentDay && 
                        Math.Floor(delivery.ArrivalDay) > currentDay - Factors.RoundIncrement &&
                        delivery.Processed == false)
                    {
                        Inventory += delivery.Volume;
                        delivery.Processed = true;
                        Payments.Add(new Payment()
                        {
                            Amount = delivery.Price * -1,
                            DueDay = delivery.ArrivalDay + Factors.RoundIncrement,
                            FromPlayer = true
                        });
                    }
                }
            }
        }

        /**
         * <summary>Gets all outgoing payments for received deliveries</summary>
         * <returns>List with payment objects that is used to pay suppliers</returns>
         * <remarks>
         * Because of the ProcessDeliveries method Payment objects will be added to the Payments list. For all these
         * items the FromPlayer bool is set to true. And the payment amount is a negative double. In this method we
         * search for these Payment Items, then flip their amount from negative to positive. And lastly return a list
         * of Payment objects
         * </remarks>
         * <param name="currentDay">integer that specifies the current day</param>
         * <param name="playerId">string that specifies the player id</param>
         */
        //TODO: test if you get double payments in db
        public List<Payment> GetOutgoingPayments(int currentDay, string playerId)
        {
            List<Payment> payments = new List<Payment>();

            for (int i = 0; i < Payments.Count; i++)
            {
                if (Payments[i].FromPlayer && (int) Payments[i].DueDay <= currentDay &&
                    (int) Payments[i].DueDay > currentDay - Factors.RoundIncrement)
                {
                    payments.Add(new Payment()
                    {
                        Amount = Payments[i].Amount * -1,
                        DueDay = Payments[i].DueDay + Factors.RoundIncrement,
                        FromPlayer = true,
                        PlayerId = playerId,
                        Id = Guid.NewGuid().ToString()
                    });
                }
            }

            return payments;
        }

        /**
         * <summary>Adds payment objects to the Payments list which are classified as transportation costs</summary>
         * <remarks>List orders must be a list of fulfilled orders</remarks>
         * <param name="currentDay">double that specifies the current day</param>
         * <param name="deliveries">List of fulfilled orders</param>
         */
        public void SetTransportCosts(double currentDay, List<Delivery> deliveries)
        {
            foreach (Delivery delivery in deliveries)
            {
                double transportDays = (delivery.ArrivalDay - currentDay);

                if (Role.Id == "Farmer")
                {
                    Payments.Add(new Payment()
                    {
                        Amount = transportDays * Factors.FarmerTransport * -1, DueDay = currentDay, FromPlayer = false,
                        PlayerId = this.Id, Id = Guid.NewGuid().ToString()
                    });
                }
                else if (Role.Id == "Processor")
                {
                    Payments.Add(new Payment()
                    {
                        Amount = transportDays * Factors.ProcTransport * -1, DueDay = currentDay, FromPlayer = false,
                        PlayerId = this.Id, Id = Guid.NewGuid().ToString()
                    });
                }
                else if (Role.Id == "Manufacturer")
                {
                    Payments.Add(new Payment()
                    {
                        Amount = transportDays * Factors.ManuTransport * -1, DueDay = currentDay, FromPlayer = false,
                        PlayerId = this.Id, Id = Guid.NewGuid().ToString()
                    });
                }
                else
                {
                    Payments.Add(new Payment()
                    {
                        Amount = transportDays * Factors.RetailTransport * -1, DueDay = currentDay, FromPlayer = false,
                        PlayerId = this.Id, Id = Guid.NewGuid().ToString()
                    });
                }
            }
        }

        /**
         * <summary>Adds a holding cost payment to the Payments list </summary>
         * <param name="currentDay">integer that specifies the current day</param>
         */
        public void SetHoldingCost(int currentDay)
        {
            Payments.Add(new Payment()
            {
                Amount = HoldingCosts * -1, DueDay = currentDay, FromPlayer = false, PlayerId = this.Id,
                Id = Guid.NewGuid().ToString()
            });
        }

        /**
         * <summary>Updates player balance</summary>
         * <param name="currentDay">integer that specifies the current day</param>
         */
        public void UpdateBalance(int currentDay)
        {
            Console.WriteLine("balance before updating" + Balance);
            for (int i = 0; i < Payments.Count; i++)
            {
                if ((int) Payments[i].DueDay <= currentDay &&
                    (int) Payments[i].DueDay > currentDay - Factors.RoundIncrement)
                {
                    Console.WriteLine("Current day: " + currentDay);
                    Console.WriteLine("Due day: " + Payments[i].DueDay);
                    Console.WriteLine("Payment id: " + Payments[i].Id);
                    Balance += Payments[i].Amount;
                }
            }
            Console.WriteLine("balance after updating" + Balance);
        }
    }
}