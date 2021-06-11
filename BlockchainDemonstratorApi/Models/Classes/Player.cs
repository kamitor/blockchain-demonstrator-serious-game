using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlockchainDemonstratorApi.Models.Classes
{
	public class Player
	{
		[Key]
		public string Id { get; set; }

		[Required]
		public string Name { get; set; }

		public virtual Role Role { get; set; }

		public double Profit
		{
			get { return Balance - (Factors.InitialCapital - Factors.SetupCost); }
		}

		public int Inventory { get; set; } = Factors.DefaultInventory;

		public double Margin { get; set; }

		private Option _chosenOption;

		public virtual Option ChosenOption
		{
			get { return _chosenOption; }
			set { _chosenOption = value; }
		}

		public double MarginCalculator(int currentDay)
		{
			return Margin = Payments
				.Where(p => p.FromPlayer && p.DueDay <= currentDay && p.DueDay > currentDay - Factors.RoundIncrement)
				.Sum(p => p.Amount);
		}

		/* Old shite
		 int backorder = 0;
		 int maxOrderDay = 0;
		 if(IncomingOrders.Count != 0) maxOrderDay = IncomingOrders.Max(o => o.OrderDay);
		 if (IncomingOrders.Count == 1)
		 {
			 backorder = IncomingOrders[0].Volume - IncomingOrders[0].Deliveries.Sum(x => x.Volume);
		 }
		 else
		 {
			 foreach (Order incomingOrder in IncomingOrders)
			 {
				 if(incomingOrder.OrderDay != maxOrderDay) backorder += incomingOrder.Volume - incomingOrder.Deliveries.Sum(x => x.Volume);
			 } 
		 }
		 return backorder;
		 */
		[NotMapped]
		public int Backorder { get; set; }

		public virtual Order CurrentOrder { get; set; }

		private List<Order> _outgoingOrders;

		/// <summary>
		/// The orders from the player itself
		/// </summary>
		[ForeignKey("OutgoingOrderForPlayerId")]
		public virtual List<Order> OutgoingOrders
		{
			get { return _outgoingOrders; }
			set { _outgoingOrders = value.OrderBy(o => o.OrderDay).ToList(); }
		}

		private List<Order> _incomingOrders;

		/// <summary>
		/// Order sent from your customer
		/// </summary>
		[ForeignKey("IncomingOrderForPlayerId")]
		public virtual List<Order> IncomingOrders
		{
			get { return _incomingOrders; }
			set { _incomingOrders = value.OrderBy(o => o.OrderDay).ToList(); }
		}

		private List<Payment> _payments;

		[ForeignKey("PlayerId")]
		public virtual List<Payment> Payments
		{
			get { return _payments; }
			set { _payments = value.OrderBy(o => o.DueDay).ThenBy(o => o.Topic).ToList(); }
		}

		public double Balance { get; set; }

		[NotMapped]
		public double HoldingCosts
		{
			get
			{
				//TODO: Change to new formula
				//running cost= (volume of inventory* holding cost factor)+ (backorder factor* backorder* holding cost)+ (incoming order* holding cost) 
				return (Inventory * Factors.HoldingFactor) +
				       (Factors.HoldingFactor * 2 * Backorder) /*+ (IncomingOrder.Volume * holdingFactor)*/;
			}
		}

		public string InventoryHistoryJson { get; set; }
		[NotMapped]
		public List<int> InventoryHistory
        {
            get { return (InventoryHistoryJson == null) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(InventoryHistoryJson); }
            set { InventoryHistoryJson = JsonConvert.SerializeObject(value); }
        }

		public string OrderWorthHistoryJson { get; set; }
		[NotMapped]
		public List<double> OrderWorthHistory
		{
			get { return (OrderWorthHistoryJson == null) ? new List<double>() : JsonConvert.DeserializeObject<List<double>>(OrderWorthHistoryJson); }
			set { OrderWorthHistoryJson = JsonConvert.SerializeObject(value); }
		}

		public string OverallProfitHistoryJson { get; set; }
		[NotMapped]
		public List<double> OverallProfitHistory
		{
			get { return (OverallProfitHistoryJson == null) ? new List<double>() : JsonConvert.DeserializeObject<List<double>>(OverallProfitHistoryJson); }
			set { OverallProfitHistoryJson = JsonConvert.SerializeObject(value); }
		}

		public string GrossProfitHistoryJson { get; set; }
		[NotMapped]
		public List<double> GrossProfitHistory
		{
			get { return (GrossProfitHistoryJson == null) ? new List<double>() : JsonConvert.DeserializeObject<List<double>>(GrossProfitHistoryJson); }
			set { GrossProfitHistoryJson = JsonConvert.SerializeObject(value); }
		}

		public Player(string name)
		{
			Id = Guid.NewGuid().ToString();
			Name = name;
			IncomingOrders = new List<Order>();
			OutgoingOrders = new List<Order>();
			Payments = new List<Payment>();
		}

		[JsonConstructor]
		public Player(string name, string playerId)
		{
			Id = playerId;
			Name = name;
			IncomingOrders = new List<Order>();
			OutgoingOrders = new List<Order>();
			Payments = new List<Payment>();
		}

		//TODO: doc blocks are incorrect
		/// <summary>Gets list of outgoing deliveries</summary>
		/// <returns>List of Order objects with available stock</returns>
		/// <param name="currentDay">integer that specifies the current day</param>
		public void GetOutgoingDeliveries(int currentDay)
		{
			Backorder = 0;
			for (int i = 0; i < IncomingOrders.Count; i++)
			{
				int leadTimeRand = new Random().Next(Factors.OrderLeadTimeRandomMinimum,
					Factors.OrderLeadTimeRandomMaximum + 1);

				int pendingVolume = IncomingOrders[i].Volume - IncomingOrders[i].Deliveries.Sum(d => d.Volume);
				if (pendingVolume <= Inventory)
				{
					Delivery delivery = new Delivery()
					{
						Volume = pendingVolume,
						SendDeliveryDay = currentDay,
						ArrivalDay = currentDay + Role.LeadTime + ChosenOption.LeadTime + leadTimeRand,
						Price = pendingVolume * Role.ProductPrice
					};
					IncomingOrders[i].Deliveries.Add(delivery);

					GetPaidForDelivery(delivery);
					AddTransportCost(currentDay, leadTimeRand);
					AddMaintenanceCost(currentDay, leadTimeRand);

					IncomingOrders.RemoveAt(i);
					Inventory -= pendingVolume;
					i--;
				}
				else if (Inventory > 0)
				{
					Delivery delivery = new Delivery()
					{
						Volume = Inventory,
						SendDeliveryDay = currentDay,
						ArrivalDay = currentDay + Role.LeadTime + ChosenOption.LeadTime + leadTimeRand,
						Price = Inventory * Role.ProductPrice
					};
					IncomingOrders[i].Deliveries.Add(delivery);
					

					GetPaidForDelivery(delivery);
					AddTransportCost(currentDay, leadTimeRand);
					AddMaintenanceCost(currentDay, leadTimeRand);

					Inventory = 0;
				}
			}
			
			foreach (Order incomingOrder in IncomingOrders)
			{
				Backorder += incomingOrder.Volume - incomingOrder.Deliveries.Sum(x => x.Volume);
			}
		}

		/// <summary>Adds the volume of incoming deliveries to inventory where arrival day is in the current round</summary>
		/// <remarks>Also adds a payment object to the Payments list for each received delivery</remarks>
		/// <param name="currentDay">integer that specifies the current day</param>
		public void ProcessDeliveries(int currentDay) //Reworked to new order system
		{
			foreach (Order order in OutgoingOrders)
			{
				foreach (Delivery delivery in order.Deliveries)
				{
					if (Math.Floor(delivery.ArrivalDay) <= currentDay &&
					    Math.Floor(delivery.ArrivalDay) > currentDay - Factors.RoundIncrement &&
					    delivery.Processed == false)
					{
						Inventory += delivery.Volume;
						delivery.Processed = true;
						Payments.Add(new Payment()
						{
							Amount = delivery.Price * -1,
							DueDay = delivery.ArrivalDay + Factors.RoundIncrement,
							FromPlayer = true,
							Topic = "Order"
						});
					}
				}
			}
		}

		/// <summary>
		/// Adds payment object to Payments for a completed delivery
		/// </summary>
		/// <param name="delivery">Delivery object for which this player has to get paid</param>
		public void GetPaidForDelivery(Delivery delivery)
		{
			Payments.Add(new Payment()
			{
				Amount = delivery.Price,
				DueDay = delivery.ArrivalDay + (2 * Factors.RoundIncrement),
				FromPlayer = true,
				PlayerId = this.Id,
				Topic = "Delivery"
			});
		}

		/// <summary>Adds payment objects to the Payments list which are classified as transportation costs</summary>
		/// <param name="currentDay">double that specifies the current day</param>
		/// <param name="leadTimeChange">Specifies the amount the lead time has changed</param>
		public void AddTransportCost(double currentDay, double leadTimeChange)
		{
			Payments.Add(new Payment()
			{
				Amount = (ChosenOption.TransportCostOneTrip + (ChosenOption.TransportCostPerDay * leadTimeChange)) * -1,
				DueDay = currentDay, FromPlayer = false,
				PlayerId = this.Id,
				Topic = "Transport"
			});
		}

		/// <summary>
		/// Adds the maintenance cost to the players Payments list
		/// </summary>
		/// <param name="currentDay">double that specifies the current day</param>
		/// <param name="leadTimeChange">the amount the lead time has changed for the current outgoing delivery</param>
		public void AddMaintenanceCost(double currentDay, double leadTimeChange)
		{
			if (leadTimeChange > 0)
			{
				Payments.Add(new Payment()
				{
					Amount = -1 * leadTimeChange * ChosenOption.CostOfMaintenance,
					DueDay = currentDay, FromPlayer = false, PlayerId = this.Id, Topic = "Maintenance"
				});
			}
		}

		/// <summary>
		/// Adds the flexibility payment to the player if the amount paid is more than 0
		/// </summary>
		/// <remarks>
		/// this method should be called each round
		/// </remarks>
		/// <param name="currentDay">double that specifies the current day</param>
		public void AddFlexibility(double currentDay)
		{
			if (ChosenOption.Flexibility > 0)
			{
				Payments.Add(new Payment
				{
					Amount = ChosenOption.Flexibility,
					DueDay = currentDay,
					FromPlayer = false,
					PlayerId = this.Id,
					Topic = "Flexibility"
				});
			}
		}

		/// <summary>
		/// Adds a payment to the Payments list 
		/// </summary>
		/// <param name="currentDay">The current day</param>
		public void AddPenalty(int currentDay)
		{
			Payments.Add(new Payment()
			{
				Amount = ChosenOption.GuaranteedCapacityPenalty * -1,
				DueDay = currentDay,
				FromPlayer = false,
				PlayerId = this.Id,
				Topic = "Penalty"
			});
		}

		/// <summary>Adds a holding cost payment to the Payments list </summary>
		/// <param name="currentDay">integer that specifies the current day</param>
		//TODO: HoldingCost can be zero (adds extra records in database)
		public void SetHoldingCost(int currentDay)
		{
			if (HoldingCosts > 0)
			{
				Payments.Add(new Payment()
				{
					Amount = HoldingCosts * -1,
					DueDay = currentDay,
					FromPlayer = false,
					PlayerId = this.Id,
					Topic = "Holding cost"
				});
			}
		}

		/// <summary>Updates player balance</summary>
		/// <param name="currentDay">integer that specifies the current day</param>
		public void UpdateBalance(int currentDay)
		{
			for (int i = 0; i < Payments.Count; i++)
			{
				if ((int) Payments[i].DueDay <= currentDay &&
				    (int) Payments[i].DueDay > currentDay - Factors.RoundIncrement)
				{
					Balance += Payments[i].Amount;
				}
			}
		}
	}
}