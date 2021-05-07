using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Delivery
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string OrderId { get; set; }

        [Required]
        public int Volume { get; set; }

        [Required]
        public int SendDeliveryDay { get; set; }

        [Required]
        public double ArrivalDay { get; set; }

        [Required]
        public bool Processed { get; set; } = false;
    }
}
