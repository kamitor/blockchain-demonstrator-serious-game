using Blockchain_Demonstrator_Web_App.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models.Interfaces
{
    public interface IRole
    {
        public Role Destination { get; set; }
        public int LeadTime { get; set; }
        public Dictionary<Option, IOption> Options { get; set; }
        public Product Product { get; set; }
    }
}
