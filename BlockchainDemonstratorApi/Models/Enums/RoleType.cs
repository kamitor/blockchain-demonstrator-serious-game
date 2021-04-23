using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Enums
{
    public enum RoleType
    {
        Retailer,
        Manufacturer,
        Processor,
        Farmer,
        Customer
    }

    public static class RoleMethods
    {
        public static RoleType DeliverTo(this RoleType role)
        {
            switch (role)
            {
                case RoleType.Retailer:
                    return RoleType.Customer;
                case RoleType.Manufacturer:
                    return RoleType.Retailer;
                case RoleType.Processor:
                    return RoleType.Manufacturer;
                case RoleType.Farmer:
                    return RoleType.Processor;
                default:
                    return RoleType.Customer;
            }
        }
    }
}
