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
        [Key] public string Id { get; set; }
        [Required] public string Name { get; set; }
        public Role Role { get; set; }
        public double Profit { get { return Balance - (double)(Factors.InitialCapital - Factors.SetupCost); }}


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
        [NotMapped] private List<Order> _incomingOrders;

        [ForeignKey("RequestForPlayerId")]
        public List<Order> IncomingOrders //sent from your customer
        {
            get { return _incomingOrders; }
            set { _incomingOrders = value.OrderBy(o => o.OrderDay).ToList(); }
        }

        [NotMapped] private List<Order> _incomingDeliveries;

        [ForeignKey("DeliveryToPlayerId")]
        public List<Order> IncomingDeliveries //sent from your supplier
        {
            get { return _incomingDeliveries; }
            set { _incomingDeliveries = value.OrderBy(o => o.ArrivalDay).ToList(); }
        }

        [NotMapped] private List<Order> _orderHistory;

        [ForeignKey("HistoryOfPlayerId")]
        public List<Order> OrderHistory
        {
            get { return _orderHistory; }
            set { _orderHistory = value.OrderBy(o => o.OrderNumber).ToList(); }
        }

        [ForeignKey("PlayerId")] public List<Payment> Payments { get; set; }
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
            IncomingDeliveries = new List<Order>();
            Payments = new List<Payment>();
        }

        [JsonConstructor]
        public Player(string name, string playerId)
        {
            Id = playerId;
            Name = name;
            IncomingOrders = new List<Order>();
            IncomingDeliveries = new List<Order>();
            Payments = new List<Payment>();
        }

        /**
         * <summary>Gets list of outgoing deliveries</summary>
         * <returns>List of Order objects with available stock</returns>
         * <param name="currentDay">integer that specifies the current day</param>
         */
        //TODO: check if you can get paid here instead of in GetOutGoingPayments()
        public List<Order> GetOutgoingDeliveries(int currentDay)
        {
            List<Order> outgoingDeliveries = new List<Order>();

            for (int i = 0; i < IncomingOrders.Count; i++)
            {
                if (IncomingOrders[i].Volume <= Inventory)
                {
                    Inventory -= IncomingOrders[i].Volume;
                    IncomingOrders[i].ArrivalDay = Role.LeadTime + currentDay /*+ new Random().Next(0, 4)*/;
                    outgoingDeliveries.Add(IncomingOrders[i]);
                    IncomingOrders.RemoveAt(i);
                    i--;
                }
                else if (Inventory > 0)
                {
                    outgoingDeliveries.Add(new Order()
                    {
                        OrderDay = IncomingOrders[i].OrderDay,
                        ArrivalDay = Role.LeadTime + currentDay /*+ new Random().Next(0, 4)*/,
                        Volume = Inventory
                    });

                    IncomingOrders[i].Volume -= Inventory;
                    Inventory = 0;

                    break;
                }
            }

            SetTransportCosts(currentDay, outgoingDeliveries);

            return outgoingDeliveries;
        }

        /**
         * <summary>Adds the volume of incoming deliveries to inventory where arrival day is in the current round</summary>
         * <remarks>Also adds a payment object to the Payments list for each received delivery</remarks>
         * <param name="currentDay">integer that specifies the current day</param>
         */
        //TODO: test for payment bugs
        public void ProcessDeliveries(int currentDay)
        {
            for (int i = 0; i < IncomingDeliveries.Count; i++)
            {
                if ((int) IncomingDeliveries[i].ArrivalDay <= currentDay &&
                    (int) IncomingDeliveries[i].ArrivalDay > currentDay - Factors.RoundIncrement)
                {
                    Inventory += IncomingDeliveries[i].Volume;
                    Payments.Add(new Payment()
                    {
                        Amount = IncomingDeliveries[i].Price * -1,
                        DueDay = IncomingDeliveries[i].ArrivalDay + Factors.RoundIncrement,
                        FromPlayer = true
                    });
                }
            }
        }

        /**
         * <summary>Gets all outgoing payments for received deliveries</summary>
         * <returns>List with payment objects that is used to pay suppliers</returns>
         * <param name="currentDay">integer that specifies the current day</param>
         * <param name="playerId">string that specifies the player id</param>
         */
        //TODO: test if you get double payments in db
        //TODO: test if payments work correctly
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
         * <param name="orders">List of fulfilled orders</param>
         */
        public void SetTransportCosts(double currentDay, List<Order> orders)
        {
            foreach (Order item in orders)
            {
                double transportDays = (item.ArrivalDay - currentDay);

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
                
                //TODO: Some payments aren't collected and some are twice
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