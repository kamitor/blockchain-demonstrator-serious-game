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
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Role Role { get; set; }
        public int Inventory { get; set; } = 20;
        //TODO: backorder is now volume of every order
        public int Backorder 
        {
            get
            {
                return IncomingOrders.Sum(o => o.Volume);
            }
        }
        public List<Order> IncomingOrders { get; set; } //sent from your customer
        public Order CurrentOrder { get; set; }
        public List<Order> IncomingDeliveries { get; set; } //sent from your supplier
        public double Balance { get; set; }
        //TODO: ProductPrice does not belong to the user, it is something that should either be placed in the role class or in a new product class. Update database with migrations when changed!
        public double ProductPrice { get; set; } //Price of product per volume
        public double holdingFactor = 1;
        [NotMapped]
        public double RunningCosts
        {
            get
            {
                //running cost= (volume of inventory* holding cost factor)+ (backorder factor* backorder* holding cost)+ (incoming order* holding cost) 
                return (Inventory * holdingFactor) + (holdingFactor * 2 * Backorder * holdingFactor) /*+ (IncomingOrder.Volume * holdingFactor)*/; //TODO: implement factors 
            }
        }

        public Player(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            IncomingOrders = new List<Order>();
            IncomingDeliveries = new List<Order>();
        }

        /**
         * <summary>Gets list of outgoing deliveries</summary>
         * <returns>List of Order objects with available stock</returns>
         * <param name="currentDay">integer that specifies the current day</param>
         */
        public List<Order> GetOutgoingDeliveries(int currentDay) //TODO: rework to send only possible outgoing deliveries
        {
            List<Order> outgoingDeliveries = new List<Order>();
            
            for (int i = 0; i < IncomingOrders.Count; i++)
            {
                if(IncomingOrders[i].Volume <= Inventory)
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
         */
        public void IncreaseInventory(int currentday)
        {
            Inventory += IncomingDeliveries
                .Where(d => d.ArrivalDay < currentday && d.ArrivalDay > currentday - 5) //Todo: make 5 a changeable factor later
                .Sum(d => d.Volume);
        }
    }
}
