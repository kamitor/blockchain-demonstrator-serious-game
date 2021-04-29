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
        [Key] public string Id { get; set; }
        public Phase CurrentPhase { get; set; }
        public int CurrentDay { get; set; }
        private Player _retailer;

        public Player Retailer
        {
            get { return _retailer; }
            set
            {
                if (value != null)
                {
                    if (value.Role.Id != "Retailer")
                        throw new ArgumentException("Given role id does not match the expected role Retailer");
                    Players.Add(value);
                    _retailer = value;
                }
            }
        }

        private Player _manufacturer;

        public Player Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                if (value != null)
                {
                    if (value.Role.Id != "Manufacturer")
                        throw new ArgumentException("Given role id does not match the expected role Manufacturer");
                    Players.Add(value);
                    _manufacturer = value;
                }
            }
        }

        private Player _processor;

        public Player Processor
        {
            get { return _processor; }
            set
            {
                if (value != null)
                {
                    if (value.Role.Id != "Processor")
                        throw new ArgumentException("Given role id does not match the expected role Processor");
                    Players.Add(value);
                    _processor = value;
                }
            }
        }

        private Player _farmer;

        public Player Farmer
        {
            get { return _farmer; }
            set
            {
                if (value != null)
                {
                    if (value.Role.Id != "Farmer")
                        throw new ArgumentException("Given role id does not match the expected role Farmer");
                    Players.Add(value);
                    _farmer = value;
                }
            }
        }

        [NotMapped] public List<Player> Players { get; set; }

        public Game()
        {
            Players = new List<Player>();
            Id = Guid.NewGuid().ToString(); //TODO: Write simple algorithm for unique id's
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
            SendPayments();
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

            // Adding order number
            Retailer.CurrentOrder.OrderNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(CurrentDay) / 5));
            Manufacturer.CurrentOrder.OrderNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(CurrentDay) / 5));
            Processor.CurrentOrder.OrderNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(CurrentDay) / 5));
            Farmer.CurrentOrder.OrderNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(CurrentDay) / 5));

            // Adding Price
            Retailer.CurrentOrder.Price = Manufacturer.ProductPrice * Retailer.CurrentOrder.Volume;
            Manufacturer.CurrentOrder.Price = Processor.ProductPrice * Manufacturer.CurrentOrder.Volume;
            Processor.CurrentOrder.Price = Farmer.ProductPrice * Processor.CurrentOrder.Volume;
            //TODO: implement better way for farmer to place orders
            Farmer.CurrentOrder.Price = 2080 * Farmer.CurrentOrder.Volume;

            // Making new order
            Retailer.IncomingOrders.Add(new Order() {OrderDay = CurrentDay, Volume = new Random().Next(5, 15)});
            Manufacturer.IncomingOrders.Add(Retailer.CurrentOrder);
            Processor.IncomingOrders.Add(Manufacturer.CurrentOrder);
            Farmer.IncomingOrders.Add(Processor.CurrentOrder);
        }

        private void SendPayments()
        {
            int customerOrderVolume = new Random().Next(5, 15);
            Manufacturer.Payments.AddRange(Retailer.GetOutgoingPayments(CurrentDay));
            Processor.Payments.AddRange(Manufacturer.GetOutgoingPayments(CurrentDay));
            Farmer.Payments.AddRange(Processor.GetOutgoingPayments(CurrentDay));
            //TODO: implement later
            Retailer.Payments.Add(new Payment()
                {Amount = customerOrderVolume * Retailer.ProductPrice, DueDay = CurrentDay + 2, toPlayer = true});
        }

        /**
         * <summary>Adds outgoing deliveries to the IncomingDelivery list</summary>
         */
        private void SendDeliveries()
        {
            Retailer.GetOutgoingDeliveries(CurrentDay);
            Retailer.IncomingDeliveries.AddRange(Manufacturer.GetOutgoingDeliveries(CurrentDay));
            Manufacturer.IncomingDeliveries.AddRange(Processor.GetOutgoingDeliveries(CurrentDay));
            Processor.IncomingDeliveries.AddRange(Farmer.GetOutgoingDeliveries(CurrentDay));
            //TODO: Implement later
            Farmer.IncomingDeliveries.Add(new Order()
            {
                OrderDay = CurrentDay, ArrivalDay = CurrentDay + new Random().Next(3, 6),
                Volume = Farmer.CurrentOrder.Volume
            }); 
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