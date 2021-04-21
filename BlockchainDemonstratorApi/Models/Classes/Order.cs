using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Order
    {
        [Key]
        public string Id { get; set; }
        public int OrderDay { get; set; }
        public double ArrivalDay { get; set; }
        public int Volume { get; set; }
        
    }
}
