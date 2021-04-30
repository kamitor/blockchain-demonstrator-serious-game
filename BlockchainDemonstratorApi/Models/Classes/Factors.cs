using System;
using System.ComponentModel.DataAnnotations;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public static class Factors
    {
        //TODO: put on database so we can change stuffs
        //Transport factors
        public static int RetailTransport { get; set; } = 1086;
        public static int ManuTransport { get; set; } = 516;
        public static int ProcTransport { get; set; } = 73;
        public static int FarmerTransport { get; set; } = 177;
        
        //Holding cost factor
        public static int HoldingFactor { get; set; } = 1;

        //Round factors
        public static int RoundIncrement { get; set; } = 7;
        
        //Product price factors
        public static int RetailProductPrice { get; set; } = 4000;
        public static int ManuProductPrice { get; set; } = 3100;
        public static int ProcProductPrice { get; set; } = 2500;
        public static int FarmerProductPrice { get; set; } = 2050;
        public static int HarvesterProductPrice { get; set; } = 2000;
    }
}