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

        public double Profit
        {
            get { return Balance - (Factors.InitialCapital - Factors.SetupCost); }
        }

        public int Inventory { get; set; } = 20;

        public double Margin { get; set; }

        private Option _chosenOption;
        public Option ChosenOption {
            get
            {
                if (_chosenOption != null)
                {
                    return _chosenOption;
                }
                else
                {
                    return new Option("Basic", 0, 0, 0, 0, 0, 0);
                }
            }
            set
            {
                _chosenOption = value;
            } 
        }
    
        public double MarginCalculator(int currentDay)
        {
            return Margin = Payments
                .Where(p => p.FromPlayer && p.DueDay <= currentDay && p.DueDay > currentDay - Factors.RoundIncrement)
                .Sum(p => p.Amount);
        }

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

        /// <summary>Gets list of outgoing deliveries</summary>
        /// <returns>List of Order objects with available stock</returns>
        /// <param name="currentDay">integer that specifies the current day</param>
        public void GetOutgoingDeliveries(int currentDay)
        {
            for (int i = 0; i < IncomingOrders.Count; i++)
            {
                int pendingVolume = IncomingOrders[i].Volume - IncomingOrders[i].Deliveries.Sum(d => d.Volume);
                if (pendingVolume <= Inventory)
                {
                    Inventory -= pendingVolume;
                    int price;
                    //TODO: implement a better version
                    switch (Role.Id)
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
                        ArrivalDay = currentDay + Role.LeadTime + ChosenOption.LeadTime + new Random().Next(0, 4),
                        Price = price
                    };
                    IncomingOrders[i].Deliveries.Add(delivery);

                    GetPaidForDelivery(delivery);
                    AddTransportCost(currentDay, delivery);

                    IncomingOrders.RemoveAt(i);
                    i--;
                }
                else if (Inventory > 0)
                {
                    int price;
                    //TODO: implement a better version
                    switch (Role.Id)
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
                        ArrivalDay = currentDay + Role.LeadTime + ChosenOption.LeadTime + new Random().Next(0, 4),
                        Price = price
                    };
                    IncomingOrders[i].Deliveries.Add(delivery);

                    GetPaidForDelivery(delivery);
                    AddTransportCost(currentDay, delivery);

                    Inventory = 0;
                }
            }
        }

        /// <summary>Adds the volume of incoming deliveries to inventory where arrival day is in the current round</summary>
        /// <remarks>Also adds a payment object to the Payments list for each received delivery</remarks>
        /// <param name="currentDay">integer that specifies the current day</param>
        public void ProcessDeliveries(int currentDay) //Reworked to new order system
        {
            foreach (Order order in OutgoingOrders)
            {
                foreach (Delivery delivery in order.Deliveries)
                {
                    if (Math.Floor(delivery.ArrivalDay) <= currentDay &&
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

        /// <summary>
        /// Adds payment object to Payments for a completed delivery
        /// </summary>
        /// <param name="delivery">Delivery object for which the player has to get paid</param>
        public void GetPaidForDelivery(Delivery delivery)
        {
            Payments.Add(new Payment()
            {
                Amount = delivery.Price, Id = Guid.NewGuid().ToString(),
                DueDay = delivery.ArrivalDay + (2 * Factors.RoundIncrement), FromPlayer = true, PlayerId = this.Id
            });
        }

        /// <summary>Adds payment objects to the Payments list which are classified as transportation costs</summary>
        /// <param name="currentDay">double that specifies the current day</param>
        /// <param name="delivery">Delivery object that has been sent to the customer</param>
        //TODO: add the one trip cost of transport cost
        public void AddTransportCost(double currentDay, Delivery delivery)
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

        public void AddPenalty(double amount, int currentDay)
        {
            Payments.Add(new Payment()
            {
                Amount = amount,
                DueDay = currentDay,
                FromPlayer = false, 
                PlayerId = this.Id,
                Id = Guid.NewGuid().ToString()
            });
        }
  
        /// <summary>Adds a holding cost payment to the Payments list </summary>
        /// <param name="currentDay">integer that specifies the current day</param>
        public void SetHoldingCost(int currentDay)
        {
            Payments.Add(new Payment()
            {
                Amount = HoldingCosts * -1, DueDay = currentDay, FromPlayer = false, PlayerId = this.Id,
                Id = Guid.NewGuid().ToString()
            });
        }
        
        /// <summary>Updates player balance</summary>
        /// <param name="currentDay">integer that specifies the current day</param>
        public void UpdateBalance(int currentDay)
        {
            for (int i = 0; i < Payments.Count; i++)
            {
                if ((int) Payments[i].DueDay <= currentDay &&
                    (int) Payments[i].DueDay > currentDay - Factors.RoundIncrement)
                {
                    Balance += Payments[i].Amount;
                }
            }
        }
    }
}