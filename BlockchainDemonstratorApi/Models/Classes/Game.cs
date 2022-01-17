using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
	/// <summary>
	/// The Game class represents the beer game, making this one of the most important classes in the REST API.
	/// This class contains most of the data used during the beer game, such as players and the current day.
	/// The Game class also contains most of the methods to simulate the beer game, any other functions that simulate the game
	/// are called from this class as well.
	/// </summary>
	public class Game
	{
		[Key]
		public string Id { get; set; }

		public int CurrentDay { get; set; }
		private Player _retailer;

		public virtual Player Retailer
		{
			get { return _retailer; }
			set
			{
				if (value != null)
				{
					if (value.Role.Id != "Retailer")
						throw new ArgumentException("Given role id does not match the expected role Retailer");
					_retailer = value;
				}
				else
				{
					_retailer = value;
				}
			}
		}

		private Player _manufacturer;

		public virtual Player Manufacturer
		{
			get { return _manufacturer; }
			set
			{
				if (value != null)
				{
					if (value.Role.Id != "Manufacturer")
						throw new ArgumentException("Given role id does not match the expected role Manufacturer");
					_manufacturer = value;
				}
				else
				{
					_manufacturer = value;
				}
			}
		}

		private Player _processor;

		public virtual Player Processor
		{
			get { return _processor; }
			set
			{
				if (value != null)
				{
					if (value.Role.Id != "Processor")
						throw new ArgumentException("Given role id does not match the expected role Processor");
					_processor = value;
				}
				else
				{
					_processor = value;
				}
			}
		}

		private Player _farmer;

		public virtual Player Farmer
		{
			get { return _farmer; }
			set
			{
				if (value != null)
				{
					if (value.Role.Id != "Farmer")
						throw new ArgumentException("Given role id does not match the expected role Farmer");
					_farmer = value;
				}
				else
				{
					_farmer = value;
				}
			}
		}

		[NotMapped]
		public virtual List<Player> Players
		{
			get
			{
				List<Player> list = new List<Player>();

				if (Retailer != null) list.Add(Retailer);
				if (Manufacturer != null) list.Add(Manufacturer);
				if (Processor != null) list.Add(Processor);
				if (Farmer != null) list.Add(Farmer);

				return list;
			}
		}

		public bool GameStarted { get; set; }

		public string GameMasterId { get; set; }

		public Game(string id)
		{
			Id = id;
			CurrentDay = 1;
			GameStarted = false;
		}

		/// <summary>
		/// This is the main function of the Game class. 
		/// This function progresses the game by triggering functions inside this function.
		/// </summary>
		public void Progress()
		{
			SaveHistory();
			ProcessDeliveries();
			SendDeliveries();

			CapacityPenalty();
			SetHoldingCosts();
			AddFlexibilityReward();
			UpdateBalance();

			SendOrders();
			CurrentDay += Factors.RoundIncrement;
		}

		private void SaveHistory()
		{
			SaveInventoryHistory();
			SaveOrderWorthHistory();
			SaveOverallProfitHistory();
			SaveGrossProfitHistory();
			SaveBalanceHistory();
			SaveBackorderHistory();
		}

		private void SaveInventoryHistory()
		{
			foreach (Player player in Players)
			{
				List<int> newInventory = new List<int>() {player.Inventory};
				player.InventoryHistory = player.InventoryHistory.Concat(newInventory).ToList();
			}
		}

		private void SaveBalanceHistory()
		{
			foreach (Player player in Players)
			{
				List<double> newBalance = new List<double>() {player.Balance};
				player.BalanceHistory = player.BalanceHistory.Concat(newBalance).ToList();
			}
		}
		
		private void SaveBackorderHistory()
		{
			foreach (Player player in Players)
			{
				List<double> newBackorder = new List<double>() {player.Backorder};
				player.BackorderHistory = player.BackorderHistory.Concat(newBackorder).ToList();
			}
		}

		private void SaveOrderWorthHistory()
		{
			foreach (Player player in Players)
			{
				List<double> newOrderWorth = new List<double>()
					{player.CurrentOrder.Volume * player.Role.ProductPrice};
				player.OrderWorthHistory = player.OrderWorthHistory.Concat(newOrderWorth).ToList();
			}
		}

		private void SaveOverallProfitHistory()
		{
			foreach (Player player in Players)
			{
				List<double> newOverallProfit = new List<double>() {player.Profit};
				player.OverallProfitHistory = player.OverallProfitHistory.Concat(newOverallProfit).ToList();
			}
		}

		private void SaveGrossProfitHistory()
		{
			foreach (Player player in Players)
			{
				List<double> newGrossProfit = new List<double>()
				{
					player.OutgoingOrders.Sum(o => o.Deliveries.Sum(d => d.Price)) +
					player.Payments.Where(p => p.Topic == "Order").Sum(p => p.Amount)
				};
				player.GrossProfitHistory = player.GrossProfitHistory.Concat(newGrossProfit).ToList();
			}
		}
		
		

		/// <summary>
		/// Used to setup the game when it first starts.
		/// Method sets the base variables for each player
		/// </summary>
		public void SetupGame()
		{
			SetInitialCapital();
			SetSetupPayment();
			SetSetupDeliveries();
			SetSetupOrders();
			GameStarted = true;
			UpdateBalance();
		}

		#region Setup

		/// <summary>
		/// Adds default order to each actor
		/// </summary>
		/// <remarks>Only needs to be used at the start of each game</remarks>
		private void SetSetupOrders() //Reworked to new order system
		{
			Order orderC = new Order() {OrderDay = 1 - Factors.RoundIncrement, Volume = Factors.SetupOrderVolume};
			Retailer.IncomingOrders.Add(orderC);

			Order orderR = new Order() {OrderDay = 1 - Factors.RoundIncrement, Volume = Factors.SetupOrderVolume};
			Retailer.OutgoingOrders.Add(orderR);
			Manufacturer.IncomingOrders.Add(orderR);

			Order orderM = new Order() {OrderDay = 1 - Factors.RoundIncrement, Volume = Factors.SetupOrderVolume};
			Manufacturer.OutgoingOrders.Add(orderM);
			Processor.IncomingOrders.Add(orderM);

			Order orderP = new Order() {OrderDay = 1 - Factors.RoundIncrement, Volume = Factors.SetupOrderVolume};
			Processor.OutgoingOrders.Add(orderP);
			Farmer.IncomingOrders.Add(orderP);
		}

		/**
         * <summary>Adds default deliveries to each actor</summary>
         * <remarks>Only needs to be used at the start of each game</remarks>
         */
		private void SetSetupDeliveries() //Reworked to new order system
		{
			for (int i = 0;
				i < (int) Math.Ceiling(Manufacturer.ChosenOption.LeadTime / (double) Factors.RoundIncrement);
				i++)
			{
				Order order = new Order() {Volume = Factors.SetupDeliveryVolume};
				order.Deliveries.Add(new Delivery()
				{
					Volume = Factors.SetupDeliveryVolume,
					SendDeliveryDay =
						Convert.ToInt32(Math.Floor(Factors.RoundIncrement * i + 1 -
						                           Manufacturer.ChosenOption.LeadTime)),
					ArrivalDay = Factors.RoundIncrement * i + 1,
					Price = Factors.ManuProductPrice * Factors.SetupDeliveryVolume
				});
				Retailer.OutgoingOrders.Add(order);
			}

			for (int i = 0;
				i < (int) Math.Ceiling(Processor.ChosenOption.LeadTime / (double) Factors.RoundIncrement);
				i++)
			{
				Order order = new Order() {Volume = Factors.SetupDeliveryVolume};
				order.Deliveries.Add(new Delivery()
				{
					Volume = Factors.SetupDeliveryVolume,
					SendDeliveryDay =
						Convert.ToInt32(Math.Floor(Factors.RoundIncrement * i + 1 - Processor.ChosenOption.LeadTime)),
					ArrivalDay = Factors.RoundIncrement * i + 1,
					Price = Factors.ProcProductPrice * Factors.SetupDeliveryVolume
				});
				Manufacturer.OutgoingOrders.Add(order);
			}

			for (int i = 0; i < (int) Math.Ceiling(Farmer.ChosenOption.LeadTime / (double) Factors.RoundIncrement); i++)
			{
				Order order = new Order() {Volume = Factors.SetupDeliveryVolume};
				order.Deliveries.Add(new Delivery()
				{
					Volume = Factors.SetupDeliveryVolume,
					SendDeliveryDay =
						Convert.ToInt32(Math.Floor(Factors.RoundIncrement * i + 1 - Farmer.ChosenOption.LeadTime)),
					ArrivalDay = Factors.RoundIncrement * i + 1,
					Price = Factors.FarmerProductPrice * Factors.SetupDeliveryVolume
				});
				Processor.OutgoingOrders.Add(order);
			}

			for (int i = 0; i < (int) Math.Ceiling(1 / (double) Factors.RoundIncrement); i++)
			{
				Order order = new Order() {Volume = Factors.SetupDeliveryVolume};
				order.Deliveries.Add(new Delivery()
				{
					Volume = Factors.SetupDeliveryVolume,
					SendDeliveryDay = Factors.RoundIncrement * i,
					ArrivalDay = Factors.RoundIncrement * i + 1,
					Price = Factors.HarvesterProductPrice * Factors.SetupDeliveryVolume
				});
				Farmer.OutgoingOrders.Add(order);
			}
		}

		/**
         * <summary>Adds 250000 to each players balance</summary>
         * <remarks>Only needed at the start of each game</remarks>
         */
		private void SetInitialCapital()
		{
			foreach (Player player in Players)
			{
				player.Balance = Factors.InitialCapital;
			}
		}

		/// <summary>
		/// Adds a standard payment for the setup costs to each actors payment list
		/// </summary>
		/// <remarks>Only needs to be called once, at the start of each phase</remarks>
		private void SetSetupPayment()
		{
			foreach (Player player in Players)
			{
				player.Payments.Add(new Payment()
				{
					Amount = player.ChosenOption.CostOfStartUp * -1,
					DueDay = 1,
					FromPlayer = false,
					PlayerId = player.Id,
					Topic = "Setup"
				});
			}
		}

		#endregion

		/// <summary>
		/// Prepares the CurrentOrder of every actor
		/// Places the CurrentOrder into the IncomingOrders list for the actor
		/// </summary>
		private void SendOrders()
		{
			AddCurrentDay();
			AddOrderNumber();
			AddOrder();
		}

		/// <summary>Adds current day to each actors current order</summary>
		private void AddCurrentDay()
		{
			// Adding current day
			foreach (Player player in Players)
			{
				player.CurrentOrder.OrderDay = CurrentDay;
			}
		}

		/// <summary>
		/// Adds order number to each actors current order
		/// </summary>
		private void AddOrderNumber()
		{
			// Adding order number
			foreach (Player player in Players)
			{
				if (player.OutgoingOrders.Count != 0)
				{
					player.CurrentOrder.OrderNumber = player.OutgoingOrders.Max(o => o.OrderNumber) + 1;
				}
				else
				{
					player.CurrentOrder.OrderNumber = 1;
				}
			}
		}

		/// <summary>
		/// Adds current order to each actors supplier
		/// </summary>
		public void AddOrder()
		{
			Retailer.IncomingOrders.Add(new Order()
			{
				OrderNumber = Convert.ToInt32(Math.Ceiling((double) CurrentDay / Factors.RoundIncrement)),
				OrderDay = CurrentDay,
				Volume = new Random().Next(Factors.RetailerOrderVolumeRandomMinimum,
					Factors.RetailerOrderVolumeRandomMaximum + 1)
			});

			Retailer.OutgoingOrders.Add(Retailer.CurrentOrder);
			Manufacturer.IncomingOrders.Add(Retailer.CurrentOrder);

			Manufacturer.OutgoingOrders.Add(Manufacturer.CurrentOrder);
			Processor.IncomingOrders.Add(Manufacturer.CurrentOrder);

			Processor.OutgoingOrders.Add(Processor.CurrentOrder);
			Farmer.IncomingOrders.Add(Processor.CurrentOrder);

			Farmer.OutgoingOrders.Add(Farmer.CurrentOrder);
		}

		/// <summary>
		/// Adds a penalty for each actor if it's needed
		/// </summary>
		private void CapacityPenalty()
		{
			foreach (Player player in Players)
			{
				player.AddPenalty(CurrentDay);
			}
		}

		///<summary>
		///Processes and sends through incomingOrders
		///</summary>
		///<remarks>
		///The farmer gets his deliveries through the manually 
		///added delivery instead of the GetOutgoingDeliveries, 
		///because the harvester does not have an player class
		///</remarks>
		private void SendDeliveries()
		{
			Retailer.AddOutgoingDeliveries(CurrentDay);
			Manufacturer.AddOutgoingDeliveries(CurrentDay);
			Processor.AddOutgoingDeliveries(CurrentDay);
			Farmer.AddOutgoingDeliveries(CurrentDay);
			Farmer.CurrentOrder.Deliveries = new List<Delivery>()
			{
				new Delivery()
				{
					Volume = Farmer.CurrentOrder.Volume,
					SendDeliveryDay = CurrentDay,
					ArrivalDay = CurrentDay + 1 + Factors.RoundIncrement * 2 + new Random().Next(0, 4),
					Price = Factors.HarvesterProductPrice * Farmer.CurrentOrder.Volume
				}
			};
		}

		///<summary>
		///Triggers the ProcessDeliveries function of each actor to process their deliveries
		///</summary>
		private void ProcessDeliveries()
		{
			foreach (Player player in Players)
			{
				player.ProcessDeliveries(CurrentDay);
			}
		}


		/// <summary>
		/// Calls the UpdateBalance method for each player
		/// </summary>
		private void UpdateBalance()
		{
			foreach (Player player in Players)
			{
				player.UpdateBalance(CurrentDay);
			}
		}

		/// <summary>
		/// Adds holding cost to each players Payments list
		/// </summary>
		private void SetHoldingCosts()
		{
			foreach (Player player in Players)
			{
				player.SetHoldingCost(CurrentDay);
			}
		}

		private void AddFlexibilityReward()
		{
			foreach (Player player in Players)
			{
				player.AddFlexibility(CurrentDay);
			}
		}

		public void FlushInventory(string playerId)
		{
			Player pl = Players.FirstOrDefault(x => x.Id == playerId);
			pl?.FlushInventory(CurrentDay);
		}

		public override string ToString()
		{
			string result = "";
			
			foreach (Player player in Players)
			{
				result += player.ToString();
			}

			return result;
		}
	}
}