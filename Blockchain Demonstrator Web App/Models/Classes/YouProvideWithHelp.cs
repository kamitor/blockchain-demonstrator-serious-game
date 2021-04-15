using Blockchain_Demonstrator_Web_App.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models.Classes
{
    public class YouProvideWithHelp : IOption
    {
        public int CostOfStartUp { get; set; }
        public int CostOfMaintenance { get; set; }
        public int LeadTime { get; set; }
        public int Flexibility { get; set; }
        public int GuaranteedCapacity { get; set; }
    }
}
