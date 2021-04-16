using Blockchain_Demonstrator_Web_App.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models.Interfaces
{
    public interface IRole
    {
        public string Destination { get; set; }
        public int LeadTime { get; set; }
        public Dictionary<Option, IOption> MyProperty { get; set; }
        public Product Product { get; set; }
    }
}
