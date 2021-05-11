using System;
using System.ComponentModel.DataAnnotations;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Factors
    {
        [Key]
        public string Id { get; set; } = "DefaultFactors";
        //InitialCapital factors
        public static int InitialCapital { get; set; } = 250000;
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
        public static int SetupCost { get; set; } = 75000;
        #region DatabaseProperties
        public int retailTransport { get { return RetailTransport; } set { RetailTransport = value; } }
        public int manuTransport { get { return ManuTransport; } set { ManuTransport = value; } }
        public int procTransport { get { return ProcTransport; } set { ProcTransport = value; } }
        public int farmerTransport { get { return FarmerTransport; } set { FarmerTransport = value; } }
        public int holdingFactor { get { return HoldingFactor; } set { HoldingFactor = value; } }
        public int roundIncrement { get { return RoundIncrement; } set { RoundIncrement = value; } }
        public int retailProductPrice { get { return RetailProductPrice; } set { RetailProductPrice = value; } }
        public int manuProductPrice { get { return ManuProductPrice; } set { ManuProductPrice = value; } }
        public int procProductPrice { get { return ProcProductPrice; } set { ProcProductPrice = value; } }
        public int farmerProductPrice { get { return FarmerProductPrice; } set { FarmerProductPrice = value; } }
        public int harvesterProductPrice { get { return HarvesterProductPrice; } set { HarvesterProductPrice = value; } }
        public int setupCost { get { return SetupCost; } set { SetupCost = value; } }
        public int initialCapital { get { return InitialCapital; } set { InitialCapital = value; } }
        #endregion
    }
}