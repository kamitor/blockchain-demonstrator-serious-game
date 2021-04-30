using BlockchainDemonstratorApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Player
    {
        [Key] public string Id { get; set; }
        [Required] public string Name { get; set; }
        public Role Role { get; set; }

        public int Inventory { get; set; } = 20;

        //TODO: backorder is now volume of every order
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
        
        [NotMapped]
        private List<Order> _incomingOrders;
        [ForeignKey("RequestForPlayerId")]
        public List<Order> IncomingOrders //sent from your customer
        {
            get
            {
                return _incomingOrders;
            }
            set
            {
                _incomingOrders = value.OrderBy(o => o.OrderDay).ToList();
            }
        } 

        [NotMapped]
        private List<Order> _incomingDeliveries;
        [ForeignKey("DeliveryToPlayerId")]
        public List<Order> IncomingDeliveries //sent from your supplier
        {
            get
            {
                return _incomingDeliveries;
            }
            set
            {
                _incomingDeliveries = value.OrderBy(o => o.ArrivalDay).ToList();
            }
        }

        [NotMapped]
        private List<Order> _orderHistory;
        [ForeignKey("HistoryOfPlayerId")]
        public List<Order> OrderHistory
        {
            get
            {
                return _orderHistory;
            }
            set
            {
                _orderHistory = value.OrderBy(o => o.OrderNumber).ToList();
            }
        }

        public List<Payment> Payments { get; set; }
        public double Balance { get; set; }

        //TODO: ProductPrice does not belong to the user, it is something that should either be placed in the role class or in a new product class. Update database with migrations when changed!
        public double ProductPrice { get; set; } //Price of product per volume
        public double holdingFactor = 1;

        [NotMapped]
        public double HoldingCosts
        {
            get
            {
                //running cost= (volume of inventory* holding cost factor)+ (backorder factor* backorder* holding cost)+ (incoming order* holding cost) 
                return (Inventory * holdingFactor) +
                       (holdingFactor * 2 * Backorder) /*+ (IncomingOrder.Volume * holdingFactor)*/
                    ; //TODO: implement factors 
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

        /**
         * <summary>Gets list of outgoing deliveries</summary>
         * <returns>List of Order objects with available stock</returns>
         * <param name="currentDay">integer that specifies the current day</param>
         */
        public List<Order> GetOutgoingDeliveries(int currentDay)
        {
            List<Order> outgoingDeliveries = new List<Order>();

            for (int i = 0; i < IncomingOrders.Count; i++)
            {
                if (IncomingOrders[i].Volume <= Inventory)
                {
                    Inventory -= IncomingOrders[i].Volume;
                    IncomingOrders[i].ArrivalDay = Role.LeadTime + currentDay + new Random().Next(0, 4);
                    outgoingDeliveries.Add(IncomingOrders[i]);
                    IncomingOrders.RemoveAt(i);
                    i--;
                }
                else if (Inventory > 0)
                {
                    outgoingDeliveries.Add(new Order()
                    {
                        OrderDay = IncomingOrders[i].OrderDay,
                        ArrivalDay = Role.LeadTime + currentDay + new Random().Next(0, 4),
                        Volume = Inventory
                    });

                    IncomingOrders[i].Volume -= Inventory;
                    Inventory = 0;

                    break;
                }
            }

            return outgoingDeliveries;
        }

        /**
         * <summary>Adds the volume of incoming deliveries to inventory where arrival day is in the current round</summary>
         * <param name="currentDay">integer that specifies the current day</param>
         */
        //TODO: make currentday -5 parameter in game logic, when round time changes this will break
        //TODO: change name
        public void IncreaseInventory(int currentday)
        {
            for (int i = 0; i < IncomingDeliveries.Count; i++)
            {
                if ((int) IncomingDeliveries[i].ArrivalDay <= currentday &&
                    (int) IncomingDeliveries[i].ArrivalDay > currentday - 5)
                {
                    Inventory += IncomingDeliveries[i].Volume;
                    //TODO: send payment to supplier
                    Payments.Add(new Payment()
                    {
                        Amount = IncomingDeliveries[i].Price * -1, DueDay = IncomingDeliveries[i].ArrivalDay + 1,
                        toPlayer = true
                    });
                }
            }
        }

        public List<Payment> GetOutgoingPayments(int currentday)
        {
            List<Payment> payments = new List<Payment>();

            for (int i = 0; i < Payments.Count; i++)
            {
                if (Payments[i].toPlayer && (int) Payments[i].DueDay <= currentday &&
                    (int) Payments[i].DueDay > currentday - 5)
                {
                    payments.Add(new Payment()
                        {Amount = Payments[i].Amount * -1, DueDay = Payments[i].DueDay + 1, toPlayer = true});
                }
            }

            return payments;
        }

        //TODO: make currentday -5 parameter in game logic, when round time changes this will break
        //TODO: add transportation cost
        public void UpdateBalance(int currentday)
        {
            for (int i = 0; i < Payments.Count; i++)
            {
                if ((int) Payments[i].DueDay <= currentday && (int) Payments[i].DueDay > currentday - 5)
                {
                    Balance += Payments[i].Amount;
                }
            }
        }
    }
}