using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Option
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double CostOfStartUp { get; set; }
        [Required]
        public double CostOfMaintenance { get; set; }
        [Required]
        public double LeadTime { get; set; }
        [Required]
        public double Flexibility { get; set; }
        [Required]
        public double GuaranteedCapacity { get; set; }

        public Option(string name, double costOfStartUp, double costOfMaintenance, double leadTime, double flexibility, double guaranteedCapacity)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            CostOfStartUp = costOfStartUp;
            CostOfMaintenance = costOfMaintenance;
            LeadTime = leadTime;
            Flexibility = flexibility;
            GuaranteedCapacity = guaranteedCapacity;
        }
    }
}
