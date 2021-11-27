using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlockchainDemonstratorApi.Models.Classes
{
	/// <summary>
	/// The Factors is used to store several variables in a static manner.
	/// The class contains duplicate properties, one of which is static 
	/// and used in the code and the other not static used to store the variable in the database.
	/// The reason why the variables are duplicate, is because Entity Framework cannot store static properties.
	/// With this workaround, many variables can still be used globally, making the development proces less complicated.
	/// </summary>
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

		#region Player factors

		public static double HoldingFactor { get; set; } = 0.25;
		public static int RetailerOrderVolumeRandomMinimum { get; set; } = 5;
		public static int RetailerOrderVolumeRandomMaximum { get; set; } = 15;
		public static int DefaultInventory { get; set; } = 20;
		public static int FlushInventoryPrice { get; set; } = 20000;

		#endregion

		#region Product factors

		public static int RetailProductPrice { get; set; } = 4000;
		public static int ManuProductPrice { get; set; } = 3100;
		public static int ProcProductPrice { get; set; } = 2500;
		public static int FarmerProductPrice { get; set; } = 2050;
		public static int HarvesterProductPrice { get; set; } = 2000;

		#endregion

		#region Order factors

		public static int RatioALeadtime { get; set; } = 0;
		public static int RatioAChance { get; set; } = 50;
		public static int RatioBLeadtime { get; set; } = 7;
		public static int RatioBChance { get; set; } = 35;
		public static int RatioCLeadtime { get; set; } = 14;
		public static int RatioCChance { get; set; } = 15;

		public static int WeightedRandom()
		{
			int total = RatioAChance + RatioBChance + RatioCChance;
			int x = new Random().Next(0, total + 1);

			if (x >= total - RatioAChance)
			{
				return RatioALeadtime;
			}

			if (x >= total - RatioAChance - RatioBChance)
			{
				return RatioBLeadtime;
			}

			return RatioCLeadtime;
		}

		#endregion

		#region Database properties

		public double holdingFactor
		{
			get { return HoldingFactor; }
			set { HoldingFactor = value; }
		}

		public int roundIncrement
		{
			get { return RoundIncrement; }
			set { RoundIncrement = value; }
		}

		public int retailProductPrice
		{
			get { return RetailProductPrice; }
			set { RetailProductPrice = value; }
		}

		public int manuProductPrice
		{
			get { return ManuProductPrice; }
			set { ManuProductPrice = value; }
		}

		public int procProductPrice
		{
			get { return ProcProductPrice; }
			set { ProcProductPrice = value; }
		}

		public int farmerProductPrice
		{
			get { return FarmerProductPrice; }
			set { FarmerProductPrice = value; }
		}

		public int harvesterProductPrice
		{
			get { return HarvesterProductPrice; }
			set { HarvesterProductPrice = value; }
		}

		public int setupCost
		{
			get { return SetupCost; }
			set { SetupCost = value; }
		}

		public int initialCapital
		{
			get { return InitialCapital; }
			set { InitialCapital = value; }
		}

		public int setUpOrderVolume
		{
			get { return SetupOrderVolume; }
			set { SetupOrderVolume = value; }
		}

		public int setUpDeliveryVolume
		{
			get { return SetupDeliveryVolume; }
			set { SetupDeliveryVolume = value; }
		}

		public int retailerOrderVolumeRandomMinimum
		{
			get { return RetailerOrderVolumeRandomMinimum; }
			set { RetailerOrderVolumeRandomMinimum = value; }
		}

		public int retailerOrderVolumeRandomMaximum
		{
			get { return RetailerOrderVolumeRandomMaximum; }
			set { RetailerOrderVolumeRandomMaximum = value; }
		}

		public int defaultInventory
		{
			get { return DefaultInventory; }
			set { DefaultInventory = value; }
		}

		public int ratioALeadtime
		{
			get { return RatioALeadtime; }
			set { RatioALeadtime = value; }
		}

		public int ratioBLeadtime
		{
			get { return RatioBLeadtime; }
			set { RatioBLeadtime = value; }
		}

		public int ratioCLeadtime
		{
			get { return RatioCLeadtime; }
			set { RatioCLeadtime = value; }
		}

		public int ratioAChance
		{
			get { return RatioAChance; }
			set { RatioAChance = value; }
		}

		public int ratioBChance
		{
			get { return RatioBChance; }
			set { RatioBChance = value; }
		}

		public int ratioCChance
		{
			get { return RatioCChance; }
			set { RatioCChance = value; }
		}

		public int flushInventoryPrice
		{
			get { return FlushInventoryPrice; }
			set { FlushInventoryPrice = value; }
		}

		#endregion
	}
}