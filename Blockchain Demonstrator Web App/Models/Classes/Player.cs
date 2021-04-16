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
        public int Inventory { get; set; }
        public List<Order> BackOrder { get; set; }
        public List<Order> IncomingOrder { get; set; }
        public Order OutgoingOrder { get; set; }
        //TODO: implement runningscosts factors
        /*public int RunningCosts
        {
            get
            {
                return (Inventory + 
            }
        }*/
    }
}
