using Blockchain_Demonstrator_Web_App.Models.Enums;
using Blockchain_Demonstrator_Web_App.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models.Classes
{
    public class Retailer : IRole
    {
        public Role Destination { get; set; } = Role.Customer;
        public double LeadTime { get; set; } = 2;
        //TODO: replace dummy values
        public Dictionary<Option, IOption> Options { get; set; } = new Dictionary<Option, IOption>()
        {
            {
                Option.YouProvide, new YouProvide()
                {
                    CostOfStartUp = 0,
                    GuaranteedCapacity = 0,
                    LeadTime = 0,
                    Flexibility = 0,
                    CostOfMaintenance = 0
                }
            },
            {
                Option.TrustedParty, new TrustedParty()
                {
                    CostOfStartUp = 0,
                    GuaranteedCapacity = 0,
                    LeadTime = 0,
                    Flexibility = 0,
                    CostOfMaintenance = 0
                }
            },
            {
                Option.DLT, new Dlt()
                {
                    CostOfStartUp = 0,
                    GuaranteedCapacity = 0,
                    LeadTime = 0,
                    Flexibility = 0,
                    CostOfMaintenance = 0
                }
            },
            {
                Option.YouProvideWithHelp, new YouProvideWithHelp()
                {
                    CostOfStartUp = 0,
                    GuaranteedCapacity = 0,
                    LeadTime = 0,
                    Flexibility = 0,
                    CostOfMaintenance = 0
                }
            }
        };
        public Product Product { get; set; } = Product.Packs;
    }
}
