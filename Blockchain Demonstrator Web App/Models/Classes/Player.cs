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
        public int IncomingOrder { get; set; }
        public int CurrentOrder { get; set; }
        public List<Order> IncomingDelivery { get; set; }
        //TODO: implement runningscosts factors
        /*public int RunningCosts
        {
            get
            {
                return (Inventory + 
            }
        }*/

        public Order SendDelivery(int currentDay)
        {
            return new Order() { ArrivalDay = Role.LeadTime + currentDay, Volume = Shipment() };
        }

        public int Shipment()
        {
            int shipment = 0;
            Backorder += IncomingOrder;

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
                .Where(d => d.ArrivalDay == currentday)
                .Sum(d => d.Volume);
        }

    }
}
