using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Order
    {
        public int OrderDay { get; set; }
        public double ArrivalDay { get; set; }
        public int Volume { get; set; }
        
    }
}
