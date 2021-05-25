using System;
using System.ComponentModel.DataAnnotations;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Factors
    {
        [Key]
        public string Id { get; set; } = "DefaultFactors";
        
        #region Game Factors
        public static int RoundIncrement { get; set; } = 7;
        #endregion

        #region Setup factors
        public static int InitialCapital { get; set; } = 250000;
        public static int SetupCost { get; set; } = 75000;
        public static int SetupOrderVolume { get; set; } = 5;
        public static int SetupDeliveryVolume { get; set; } = 5;
        #endregion
        
        #region Transport factors
        //TODO: these are not used
        public static int RetailTransport { get; set; } = 1086;
        public static int ManuTransport { get; set; } = 516;
        public static int ProcTransport { get; set; } = 73;
        public static int FarmerTransport { get; set; } = 177;
        #endregion

        #region Player factors
        public static double HoldingFactor { get; set; } = 0.5;
        public static int RetailerOrderVolumeRandomMinimum { get; set; } = 5;
        public static int RetailerOrderVolumeRandomMaximum { get; set; } = 15;
        public static int DefaultInventory { get; set; } = 20;
        #endregion

        #region Product factors
        public static int RetailProductPrice { get; set; } = 4000;
        public static int ManuProductPrice { get; set; } = 3100;
        public static int ProcProductPrice { get; set; } = 2500;
        public static int FarmerProductPrice { get; set; } = 2050;
        public static int HarvesterProductPrice { get; set; } = 2000;
        #endregion

        #region Order factors
        public static int OrderLeadTimeRandomMinimum { get; set; } = 0;
        public static int OrderLeadTimeRandomMaximum { get; set; } = 3;
        #endregion

        #region Database properties
        public int retailTransport { get { return RetailTransport; } set { RetailTransport = value; } }
        public int manuTransport { get { return ManuTransport; } set { ManuTransport = value; } }
        public int procTransport { get { return ProcTransport; } set { ProcTransport = value; } }
        public int farmerTransport { get { return FarmerTransport; } set { FarmerTransport = value; } }
        public double holdingFactor { get { return HoldingFactor; } set { HoldingFactor = value; } }
        public int roundIncrement { get { return RoundIncrement; } set { RoundIncrement = value; } }
        public int retailProductPrice { get { return RetailProductPrice; } set { RetailProductPrice = value; } }
        public int manuProductPrice { get { return ManuProductPrice; } set { ManuProductPrice = value; } }
        public int procProductPrice { get { return ProcProductPrice; } set { ProcProductPrice = value; } }
        public int farmerProductPrice { get { return FarmerProductPrice; } set { FarmerProductPrice = value; } }
        public int harvesterProductPrice { get { return HarvesterProductPrice; } set { HarvesterProductPrice = value; } }
        public int setupCost { get { return SetupCost; } set { SetupCost = value; } }
        public int initialCapital { get { return InitialCapital; } set { InitialCapital = value; } }
        public int setUpOrderVolume { get { return SetupOrderVolume; } set { SetupOrderVolume = value; } }
        public int setUpDeliveryVolume { get { return SetupDeliveryVolume; } set { SetupDeliveryVolume = value; } }
        public int retailerOrderVolumeRandomMinimum { get { return RetailerOrderVolumeRandomMinimum; } set { RetailerOrderVolumeRandomMinimum = value; } }
        public int retailerOrderVolumeRandomMaximum { get { return RetailerOrderVolumeRandomMaximum; } set { RetailerOrderVolumeRandomMaximum = value; } }
        public int defaultInventory { get { return DefaultInventory; } set { DefaultInventory = value; } }
        public int orderLeadTimeRandomMinimum { get { return OrderLeadTimeRandomMinimum; } set { OrderLeadTimeRandomMinimum = value; } }
        public int orderLeadTimeRandomMaximum { get { return OrderLeadTimeRandomMaximum; } set { OrderLeadTimeRandomMaximum = value; } }
        #endregion
    }
}