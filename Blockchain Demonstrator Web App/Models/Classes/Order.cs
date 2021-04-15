using Blockchain_Demonstrator_Web_App.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models.Classes
{
    public class Order
    {
        public Product Product { get; set; }
        public int OrderWeek { get; set; }
        public int ArrivalWeek { get; set; }
        public int Volume { get; set; }
    }
}
