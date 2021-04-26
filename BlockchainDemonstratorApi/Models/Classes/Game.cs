using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Game
    {
        [Key]
        public string Id { get; set; }
        public Phase CurrentPhase { get; set; }
        public int CurrentDay { get; set; }
        private Player _retailer;
        public Player Retailer 
        {
            get
            {
                return _retailer;
            }
            set 
            {
                if (value != null) 
                {
                    if (value.Role.Id != "Retailer") throw new ArgumentException("Given role id does not match the expected role Retailer");
                    Players.Add(value);
                    _retailer = value;
                }
            }
        }
        private Player _manufacturer;
        public Player Manufacturer
        {
            get
            {
                return _manufacturer;
            }
            set
            {
                if (value != null) 
                {
                    if (value.Role.Id != "Manufacturer") throw new ArgumentException("Given role id does not match the expected role Manufacturer");
                    Players.Add(value);
                    _manufacturer = value;
                }
            }
        }
        private Player _processor;
        public Player Processor
        {
            get
            {
                return _processor;
            }
            set
            {
                if (value != null) 
                {
                    if (value.Role.Id != "Processor") throw new ArgumentException("Given role id does not match the expected role Processor");
                    Players.Add(value);
                    _processor = value;
                }
            }
        }
        private Player _farmer;
        public Player Farmer
        {
            get
            {
                return _farmer;
            }
            set
            {
                if (value != null) 
                {
                    if (value.Role.Id != "Farmer") throw new ArgumentException("Given role id does not match the expected role Farmer");
                    Players.Add(value);
                    _farmer = value;
                }
            }
        }
        [NotMapped]
        public List<Player> Players { get; set; }

        public Game()
        {
            Players = new List<Player>();
            Id = Guid.NewGuid().ToString();  //TODO: Write simple algorithm for unique id's
            CurrentPhase = Phase.Phase1;
            CurrentDay = 1;
        }

        /**
         * <summary>Makes game Progress to next round</summary>
         */
        public void Progress()
        {
            SendOrders();
            ProcessDeliveries();
            SendDeliveries();
           
            CurrentDay += 5; 
        }

        /**
         * <summary>Sets IncomingOrder for every actor</summary>
         */
        //TODO: seperate functions (SOLID)
        private void SendOrders()
        {
            // Adding current day
            Retailer.CurrentOrder.OrderDay = CurrentDay;
            Manufacturer.CurrentOrder.OrderDay = CurrentDay;
            Processor.CurrentOrder.OrderDay = CurrentDay;
            Farmer.CurrentOrder.OrderDay = CurrentDay;
            
            // Adding Price
            Retailer.CurrentOrder.Price = Manufacturer.ProductPrice * Retailer.CurrentOrder.Volume;
            Manufacturer.CurrentOrder.Price = Processor.ProductPrice * Manufacturer.CurrentOrder.Volume;
            Processor.CurrentOrder.Price = Farmer.ProductPrice * Processor.CurrentOrder.Volume;
            Farmer.CurrentOrder.Price = 2080 * Farmer.CurrentOrder.Volume;
            
            // Paying supplier
            Retailer.Balance -= Retailer.CurrentOrder.Price;
            Manufacturer.Balance -= Manufacturer.CurrentOrder.Price;
            Processor.Balance -= Processor.CurrentOrder.Price;
            Farmer.Balance -= Farmer.CurrentOrder.Price;

            // Getting payed
            int customerOrderVolume = new Random().Next(5, 15);  
            Manufacturer.Balance += Retailer.CurrentOrder.Price;
            Processor.Balance += Manufacturer.CurrentOrder.Price;
            Farmer.Balance += Processor.CurrentOrder.Price;
            Retailer.Balance += customerOrderVolume * Retailer.ProductPrice;  
            
            // Making new order
            Retailer.IncomingOrder = new Order() { OrderDay = CurrentDay, Volume = customerOrderVolume }; 
            Manufacturer.IncomingOrder = Retailer.CurrentOrder;
            Processor.IncomingOrder = Manufacturer.CurrentOrder;
            Farmer.IncomingOrder = Processor.CurrentOrder;
        }

        /**
         * <summary>Adds outgoing deliveries to the IncomingDelivery list</summary>
         */
        private void SendDeliveries()
        {
            Retailer.GetOutgoingDelivery(CurrentDay);
            Retailer.IncomingDelivery.Add(Manufacturer.GetOutgoingDelivery(CurrentDay));
            Manufacturer.IncomingDelivery.Add(Processor.GetOutgoingDelivery(CurrentDay));
            Processor.IncomingDelivery.Add(Farmer.GetOutgoingDelivery(CurrentDay));
            Farmer.IncomingDelivery.Add(new Order() { OrderDay = CurrentDay, ArrivalDay = CurrentDay + new Random().Next(3, 6), Volume = Farmer.CurrentOrder.Volume }); //TODO: Implement later
        }

        /**
         * <summary>Causes each actor to increase their inventory</summary>
         */
        private void ProcessDeliveries()
        {
            Retailer.IncreaseInventory(CurrentDay);
            Manufacturer.IncreaseInventory(CurrentDay);
            Processor.IncreaseInventory(CurrentDay);
            Farmer.IncreaseInventory(CurrentDay);
        }
    }
}
