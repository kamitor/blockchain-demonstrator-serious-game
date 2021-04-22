using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Interfaces
{
    public interface IRole
    {
        public string Id { get; set; }
        public double LeadTime { get; set; }
        public IOption YouProvide { get; set; }
        public IOption YouProvideWithHelp { get; set; }
        public IOption TrustedParty { get; set; }
        public IOption DLT { get; set; }
        public Product Product { get; set; }
    }
}
