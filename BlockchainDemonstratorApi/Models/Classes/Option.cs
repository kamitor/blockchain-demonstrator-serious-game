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
        public double TransportCostOneTrip { get; set; }
        [Required]
        public double TransportCostPerDay { get; set; }
        [Required]
        public double LeadTime { get; set; }
        [Required]
        public double Flexibility { get; set; }
        [Required]
        public double GuaranteedCapacityPenalty { get; set; }
        [ForeignKey("Role")]
        public string RoleId { get; set; }
        public static int MinimumGuaranteedCapacity { get; set; } = 8;

        public Option(string name, double costOfStartUp, double costOfMaintenance, double transportCostOneTrip, double transportCostPerDay, double leadTime, double flexibility, double guaranteedCapacityPenalty)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            CostOfStartUp = costOfStartUp;
            CostOfMaintenance = costOfMaintenance;
            TransportCostOneTrip = transportCostOneTrip;
            TransportCostPerDay = transportCostPerDay;
            LeadTime = leadTime;
            Flexibility = flexibility;
            GuaranteedCapacityPenalty = guaranteedCapacityPenalty;
        }
    }
}
