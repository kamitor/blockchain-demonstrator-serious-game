using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Interfaces
{
    public interface IOption
    {
        public int CostOfStartUp { get; set; }
        public int CostOfMaintenance { get; set; }
        public int LeadTime { get; set; }
        public int Flexibility { get; set; }
        public int GuaranteedCapacity { get; set; }
    }
}
