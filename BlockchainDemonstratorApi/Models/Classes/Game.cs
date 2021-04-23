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

        public void Progress()
        {
            SendOrders();
            CallGetDeliveries();
            CallSendDeliveries();
           
            CurrentDay += 5; 
        }

        private void SendOrders()
        {
            Retailer.CurrentOrder.OrderDay = CurrentDay;
            Manufacturer.CurrentOrder.OrderDay = CurrentDay;
            Processor.CurrentOrder.OrderDay = CurrentDay;
            Farmer.CurrentOrder.OrderDay = CurrentDay;
            
            // AddingPrice
            Retailer.CurrentOrder.Price = Manufacturer.ItemPrice * Retailer.CurrentOrder.Volume;
            Manufacturer.CurrentOrder.Price = Processor.ItemPrice * Manufacturer.CurrentOrder.Volume;
            Processor.CurrentOrder.Price = Farmer.ItemPrice * Processor.CurrentOrder.Volume;
            Farmer.CurrentOrder.Price = 2080 * Farmer.CurrentOrder.Volume;
            
            //paying 
            Retailer.Money -= Retailer.CurrentOrder.Price;
            Manufacturer.Money -= Manufacturer.CurrentOrder.Price;
            Processor.Money -= Processor.CurrentOrder.Price;
            Farmer.Money -= Farmer.CurrentOrder.Price;

            //getting payed
            int customerOrderVolume = new Random().Next(5, 15);  
            Manufacturer.Money += Retailer.CurrentOrder.Price;
            Processor.Money += Manufacturer.CurrentOrder.Price;
            Farmer.Money += Processor.CurrentOrder.Price;
            Retailer.Money += customerOrderVolume * Retailer.ItemPrice;  
            
            // making new order
            Retailer.IncomingOrder = new Order() { OrderDay = CurrentDay, Volume = customerOrderVolume }; 
            Manufacturer.IncomingOrder = Retailer.CurrentOrder;
            Processor.IncomingOrder = Manufacturer.CurrentOrder;
            Farmer.IncomingOrder = Processor.CurrentOrder;
            

        }

        private void CallSendDeliveries()
        {
            Retailer.IncomingDelivery.Add(Manufacturer.SendDelivery(CurrentDay));
            Manufacturer.IncomingDelivery.Add(Processor.SendDelivery(CurrentDay));
            Processor.IncomingDelivery.Add(Farmer.SendDelivery(CurrentDay));
            Farmer.IncomingDelivery.Add(new Order() { OrderDay = CurrentDay, ArrivalDay = CurrentDay + new Random().Next(1, 5), Volume = new Random().Next(5, 15) }); //TODO: Implement later
        }

        private void CallGetDeliveries()
        {
            Retailer.GetDeliveries(CurrentDay);
            Manufacturer.GetDeliveries(CurrentDay);
            Processor.GetDeliveries(CurrentDay);
            Farmer.GetDeliveries(CurrentDay);
        }
    }
}
