using Blockchain_Demonstrator_Web_App.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models.Classes
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IRole Role { get; set; }
        public int Inventory { get; set; } = 20;
        public int Backorder { get; set; }
        public Order IncomingOrder { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> IncomingDelivery { get; set; }
        private double _money;
        public double Money 
        {
            get
            {
                return _money;
            }
            set
            {
                _money = value; //TODO: Implement cost calculation and replace with better name
            }
        }
        // public int RunningCosts
        // {
        //     get
        //     {
        //         return (Inventory * 1) + (Backorder * 1 * 2) + (IncomingOrder.Volume * 1); //TODO: implement factors
        //     }
        // }

        public Player(string name, IRole role)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Role = role;
            IncomingDelivery = new List<Order>();
        }

        public Order SendDelivery(int currentDay)
        {
            IncomingOrder.ArrivalDay = Role.LeadTime + currentDay;
            IncomingOrder.Volume = Shipment();
            return IncomingOrder;
            //return new Order() { ArrivalDay = Role.LeadTime + currentDay, Volume = Shipment() };
        }

        public int Shipment()
        {
            int shipment = 0;
            Backorder += IncomingOrder.Volume;

            if (Inventory < Backorder)
            {
                shipment = Inventory;
                Backorder -= Inventory;
                Inventory = 0;
            }
            else
            {
                Inventory -= Backorder;
                shipment = Backorder;
                Backorder = 0;
            }

            return shipment;
        }

        public void GetDeliveries(int currentday)
        {
            Inventory += IncomingDelivery
                .Where(d => d.ArrivalDay < currentday && d.ArrivalDay > currentday - 5) //Todo: make 5 a changeable factor later
                .Sum(d => d.Volume);
        }
    }
}
