using BlockchainDemonstratorApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public class Game
    {
        [Key] 
        public string Id { get; set; }
        public Phase CurrentPhase { get; set; }
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
            }
        }

        //TODO: has bug where it is initialized twice, once during getting from database and second when serialized in web controller
        [NotMapped]
        public virtual List<Player> Players {
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

        public Game(string id)
        {
            Id = id;
            CurrentPhase = Phase.Phase1;
            CurrentDay = 1;
            GameStarted = false;
        }
        
        /// <summary>
        /// Makes game Progress to next round
        /// </summary>
        public void Progress()
        {
            if (!GameStarted)
            {
                SetInitialCapital();
                SetSetupPayment();
                SetSetupDeliveries();
                SetSetupOrders();
                GameStarted = true;
                UpdateBalance();
            }
            else
            {
                ProcessDeliveries();
                SendDeliveries();
                
                CapacityPenalty();
                SetHoldingCosts();
                UpdateBalance();

                SendOrders();
                CurrentDay += Factors.RoundIncrement;
            }
        }

        #region SetupFor1stRound
        
        /// <summary>
        /// Adds default order to each actor
        /// </summary>
        /// <remarks>Only needs to be used at the start of each game</remarks>
        private void SetSetupOrders() //Reworked to new order system
        {
            Order orderC = new Order() { OrderDay = 1 - Factors.RoundIncrement, Volume = 5 };
            Retailer.IncomingOrders.Add(orderC);

            Order orderR = new Order() { OrderDay = 1 - Factors.RoundIncrement, Volume = 5 };
            Retailer.OutgoingOrders.Add(orderR);
            Manufacturer.IncomingOrders.Add(orderR);

            Order orderM = new Order() { OrderDay = 1 - Factors.RoundIncrement, Volume = 5 };
            Manufacturer.OutgoingOrders.Add(orderM);
            Processor.IncomingOrders.Add(orderM);

            Order orderP = new Order() { OrderDay = 1 - Factors.RoundIncrement, Volume = 5 };
            Processor.OutgoingOrders.Add(orderP);
            Farmer.IncomingOrders.Add(orderP);
        }

        /**
         * <summary>Adds default deliveries to each actor</summary>
         * <remarks>Only needs to be used at the start of each game</remarks>
         */
        private void SetSetupDeliveries() //Reworked to new order system
        {
            for (int i = 0; i < (int)Math.Ceiling(Manufacturer.Role.LeadTime / (double)Factors.RoundIncrement); i++)
            {
                Order order = new Order() { Volume = 5 };
                order.Deliveries.Add(new Delivery() {
                    Volume = 5, 
                    SendDeliveryDay = Convert.ToInt32(Math.Floor(Factors.RoundIncrement * i + 1 - Manufacturer.Role.LeadTime)), 
                    ArrivalDay = Factors.RoundIncrement * i + 1, 
                    Price = Factors.ManuProductPrice * 5 });
                Retailer.OutgoingOrders.Add(order);
            }

            for (int i = 0; i < (int)Math.Ceiling(Processor.Role.LeadTime / (double)Factors.RoundIncrement); i++)
            {
                Order order = new Order() { Volume = 5 };
                order.Deliveries.Add(new Delivery()
                {
                    Volume = 5,
                    SendDeliveryDay = Convert.ToInt32(Math.Floor(Factors.RoundIncrement * i + 1 - Processor.Role.LeadTime)),
                    ArrivalDay = Factors.RoundIncrement * i + 1,
                    Price = Factors.ProcProductPrice * 5
                });
                Manufacturer.OutgoingOrders.Add(order);
            }

            for (int i = 0; i < (int)Math.Ceiling(Farmer.Role.LeadTime / (double)Factors.RoundIncrement); i++)
            {
                Order order = new Order() { Volume = 5 };
                order.Deliveries.Add(new Delivery()
                {
                    Volume = 5,
                    SendDeliveryDay = Convert.ToInt32(Math.Floor(Factors.RoundIncrement * i + 1 - Farmer.Role.LeadTime)),
                    ArrivalDay = Factors.RoundIncrement * i + 1,
                    Price = Factors.FarmerProductPrice * 5
                });
                Processor.OutgoingOrders.Add(order);
            }

            for (int i = 0; i < (int)Math.Ceiling(1 / (double)Factors.RoundIncrement); i++)
            {
                Order order = new Order() { Volume = 5 };
                order.Deliveries.Add(new Delivery()
                {
                    Volume = 5,
                    SendDeliveryDay = Factors.RoundIncrement * i,
                    ArrivalDay = Factors.RoundIncrement * i + 1,
                    Price = Factors.HarvesterProductPrice * 5
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
        //TODO: make sure this method is called when new phase starts
        private void SetSetupPayment()
        {
            /*Retailer.Payments.Add(new Payment(){Amount = Factors.SetupCost, DueDay = 1, ToPlayer = false, PlayerId = Retailer.Id, Id = Guid.NewGuid().ToString()});
            Manufacturer.Payments.Add(new Payment(){Amount = Factors.SetupCost, DueDay = 1, ToPlayer = false, PlayerId = Manufacturer.Id, Id = Guid.NewGuid().ToString()});
            Processor.Payments.Add(new Payment(){Amount = Factors.SetupCost, DueDay = 1, ToPlayer = false, PlayerId = Processor.Id, Id = Guid.NewGuid().ToString()});
            Farmer.Payments.Add(new Payment(){Amount = Factors.SetupCost, DueDay = 1, ToPlayer = false, PlayerId = Farmer.Id, Id = Guid.NewGuid().ToString()});*/

            foreach (Player player in Players)
            {
                player.Payments.Add(new Payment()
                {
                    Amount = player.ChosenOption.CostOfStartUp * -1, DueDay = 1, FromPlayer = false, PlayerId = player.Id,
                    Id = Guid.NewGuid().ToString()
                });
            }
        }
        #endregion
        
        /// <summary>Sets IncomingOrder for every actor</summary>
        private void SendOrders()
        {
            AddingCurrentDay();
            AddingOrderNumber();
            AddOrder();
        }
        
        /// <summary>Adds current day to each actors current order</summary>
        public void AddingCurrentDay()
        {
            // Adding current day
            Retailer.CurrentOrder.OrderDay = CurrentDay;
            Manufacturer.CurrentOrder.OrderDay = CurrentDay;
            Processor.CurrentOrder.OrderDay = CurrentDay;
            Farmer.CurrentOrder.OrderDay = CurrentDay;
        }
        
        /// <summary>
        /// Adds order number to each actors current order
        /// </summary>
        public void AddingOrderNumber()
        {
            // Adding order number
            foreach (Player player in Players)
            {
                player.CurrentOrder.OrderNumber = player.OutgoingOrders.Count + 1;
            }
        }

        /// <summary>
        /// Adds current order to each actors supplier
        /// </summary>
        public void AddOrder()
        {
            // Making new order
            Retailer.IncomingOrders.Add(new Order() {
                OrderNumber = Convert.ToInt32(Math.Ceiling((double)(CurrentDay / Factors.RoundIncrement))),
                OrderDay = CurrentDay, 
                Volume = new Random().Next(5, 15)
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
            if (Retailer.CurrentOrder.Volume <= Option.MinimumGuaranteedCapacity)
            {
                Retailer.AddPenalty(Manufacturer.ChosenOption.GuaranteedCapacityPenalty, CurrentDay);
            }
            
            if (Manufacturer.CurrentOrder.Volume <= Option.MinimumGuaranteedCapacity)
            {
                Manufacturer.AddPenalty(Processor.ChosenOption.GuaranteedCapacityPenalty, CurrentDay);
            }
            
            if (Processor.CurrentOrder.Volume <= Option.MinimumGuaranteedCapacity)
            {
                Processor.AddPenalty(Farmer.ChosenOption.GuaranteedCapacityPenalty, CurrentDay);
            }
            
            if (Farmer.CurrentOrder.Volume <= Option.MinimumGuaranteedCapacity)
            {
                //TODO: change to actual variables
                Processor.AddPenalty(1200, CurrentDay);
            }
        }

        ///<summary>
        ///Processes and sends through incomingOrders
        ///</summary>
        private void SendDeliveries() //Reworked to new order system
        {
            Retailer.GetOutgoingDeliveries(CurrentDay);
            Manufacturer.GetOutgoingDeliveries(CurrentDay);
            Processor.GetOutgoingDeliveries(CurrentDay);
            Farmer.GetOutgoingDeliveries(CurrentDay);
            Farmer.CurrentOrder.Deliveries = new List<Delivery>() {
                    new Delivery()
                    {
                        Volume = Farmer.CurrentOrder.Volume,
                        SendDeliveryDay = CurrentDay,
                        ArrivalDay = CurrentDay + Factors.RoundIncrement + new Random().Next(0,4),
                        Price = Factors.HarvesterProductPrice * Farmer.CurrentOrder.Volume
                    }
            };
        }

        ///<summary>
        ///Causes each actor to process their deliveries
        ///</summary>
        private void ProcessDeliveries()
        {
            /*Retailer.IncreaseInventory(CurrentDay);
            Manufacturer.IncreaseInventory(CurrentDay);
            Processor.IncreaseInventory(CurrentDay);
            Farmer.IncreaseInventory(CurrentDay);*/

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
    }
}