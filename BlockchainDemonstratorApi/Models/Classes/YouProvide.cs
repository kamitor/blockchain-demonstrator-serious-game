using BlockchainDemonstratorApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class YouProvide : IOption
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public int CostOfStartUp { get; set; }
        [Required]
        public int CostOfMaintenance { get; set; }
        [Required]
        public int LeadTime { get; set; }
        [Required]
        public int Flexibility { get; set; }
        [Required]
        public int GuaranteedCapacity { get; set; }
    }
}
