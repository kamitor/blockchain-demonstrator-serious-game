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
        public Player Retailer {
            get
            {
                return _retailer;
            }
            set 
            {
                if (value != null) value.Role = new Retailer();
                _retailer = value;
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
                if (value != null) value.Role = new Manufacturer();
                _manufacturer = value;
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
                if (value != null) value.Role = new Processor();
                _processor = value;
            }
        }
        private Player _farmer;
        public Player Farmer
        {
            get
            {
                return _processor;
            }
            set
            {
                if (value != null) value.Role = new Farmer();
                _processor = value;
            }
        }
        //public Dictionary<Role, Player> Players { get; set; }

        public Game()
        {
            Id = Guid.NewGuid().ToString(); //TODO: Write simple algorithm for unique id's
            CurrentPhase = Phase.Phase1;
            CurrentDay = 1;
            //Players = new Dictionary<Role, Player>();
        }

        public void Progress()
        {
            SendOrders();
            CallGetDeliveries();
            CallSendDeliveries();
            CurrentDay += 5; //TODO: Implement day factor
        }

        private void SendOrders()
        {
            Retailer.CurrentOrder.OrderDay = CurrentDay;
            Manufacturer.CurrentOrder.OrderDay = CurrentDay;
            Processor.CurrentOrder.OrderDay = CurrentDay;
            Farmer.CurrentOrder.OrderDay = CurrentDay;

            Retailer.IncomingOrder = new Order() { OrderDay = CurrentDay, Volume = 10 }; //TODO: Implemement later new Random().Next(5,15)
            Manufacturer.IncomingOrder = Retailer.CurrentOrder;
            Processor.IncomingOrder = Manufacturer.CurrentOrder;
            Farmer.IncomingOrder = Processor.CurrentOrder;
        }

        private void CallSendDeliveries()
        {
            Retailer.IncomingDelivery.Add(Manufacturer.SendDelivery(CurrentDay));
            Manufacturer.IncomingDelivery.Add(Processor.SendDelivery(CurrentDay));
            Processor.IncomingDelivery.Add(Farmer.SendDelivery(CurrentDay));
            Farmer.IncomingDelivery.Add(new Order() { ArrivalDay = new Random().Next(1, 5), Volume = new Random().Next(5, 15) }); //TODO: Implement later
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
