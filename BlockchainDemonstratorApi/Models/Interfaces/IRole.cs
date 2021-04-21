using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Interfaces
{
    public interface IRole
    {
        public Role Destination { get; set; }
        public double LeadTime { get; set; }
        public Dictionary<Option, IOption> Options { get; set; }
        public Product Product { get; set; }
    }
}
