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
        public int Backorder { get; set; }
        public Order IncomingOrder { get; set; } //sent from your customer
        public Order CurrentOrder { get; set; }
        public List<Order> IncomingDelivery { get; set; } //sent from your supplier
        public double Balance { get; set; }
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
            IncomingDelivery = new List<Order>();
        }

        /**
         * <summary>Gets outgoing delivery</summary>
         * <returns>Order object with available stock</returns>
         */
        public Order GetOutgoingDelivery(int currentDay)
        {
            return new Order()
            {
                ArrivalDay = Role.LeadTime + currentDay,
                Volume = GetOutgoingVolume(),
                OrderDay = IncomingOrder.OrderDay
            };
        }

        /**
         * <summary>
         * Gets volume for outgoing orders also handles inventory and backorder.
         * If incoming order request more volume than we have.
         * all available stock will be send and excess will be added to backorder.
         * </summary>
         * <returns>Order Volume as an int</returns>
         */
        public int GetOutgoingVolume()
        {
            int volume = 0;
            Backorder += IncomingOrder.Volume;

            if (Inventory < Backorder)
            {
                volume = Inventory;
                Backorder -= Inventory;
                Inventory = 0;
            }
            else
            {
                Inventory -= Backorder;
                volume = Backorder;
                Backorder = 0;
            }

            return volume;
        }

        /**
         * <summary>Adds the volume of incoming deliveries to inventory where arrival day is in the current round</summary>
         */
        public void IncreaseInventory(int currentday)
        {
            Inventory += IncomingDelivery
                .Where(d => d.ArrivalDay < currentday && d.ArrivalDay > currentday - 5) //Todo: make 5 a changeable factor later
                .Sum(d => d.Volume);
        }
    }
}
