using Blockchain_Demonstrator_Web_App.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models.Classes
{
    public class Game
    {
        public string Id { get; set; }
        public Phase CurrentPhase { get; set; }
        public int CurrentDay { get; set; }
        public Dictionary<Role, Player> Players { get; set; }

        public Game()
        {
            Id = Guid.NewGuid().ToString();
            CurrentPhase = Phase.Phase1;
            CurrentDay = 1;
            Players = new Dictionary<Role, Player>();
        }

        public void Progress()
        {
            SendOrders();
            CallSendDeliveries();
            CallGetDeliveries();
            CurrentDay++;
        }

        public void SendOrders()
        {
            Players[Role.Retailer].IncomingOrder = new Random().Next(5,15); //TODO: Implemement later
            Players[Role.Manufacturer].IncomingOrder = Players[Role.Retailer].CurrentOrder;
            Players[Role.Processor].IncomingOrder = Players[Role.Manufacturer].CurrentOrder;
            Players[Role.Farmer].IncomingOrder = Players[Role.Processor].CurrentOrder;
        }

        public void CallSendDeliveries()
        {
            Players[Role.Retailer].IncomingDelivery.Add(Players[Role.Manufacturer].SendDelivery(CurrentDay));
            Players[Role.Manufacturer].IncomingDelivery.Add(Players[Role.Processor].SendDelivery(CurrentDay));
            Players[Role.Processor].IncomingDelivery.Add(Players[Role.Farmer].SendDelivery(CurrentDay));
            Players[Role.Farmer].IncomingDelivery.Add(new Order() { ArrivalDay = new Random().Next(1, 5), Volume = new Random().Next(5, 15) }); //TODO: Implement later
        }

        public void CallGetDeliveries()
        {
            Players[Role.Retailer].GetDeliveries(CurrentDay);
            Players[Role.Manufacturer].GetDeliveries(CurrentDay);
            Players[Role.Processor].GetDeliveries(CurrentDay);
            Players[Role.Farmer].GetDeliveries(CurrentDay);
        }
    }
}
