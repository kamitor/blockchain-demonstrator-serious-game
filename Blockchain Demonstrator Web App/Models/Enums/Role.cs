using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain_Demonstrator_Web_App.Models.Enums
{
    public enum Role
    {
        Retailer,
        Manufacturer,
        Processor,
        Farmer,
        Customer
    }

    public static class RoleMethods
    {
        public static Role DeliverTo(this Role role)
        {
            switch (role)
            {
                case Role.Retailer:
                    return Role.Customer;
                case Role.Manufacturer:
                    return Role.Retailer;
                case Role.Processor:
                    return Role.Manufacturer;
                case Role.Farmer:
                    return Role.Processor;
                default:
                    return Role.Customer;
            }
        }
    }
}
