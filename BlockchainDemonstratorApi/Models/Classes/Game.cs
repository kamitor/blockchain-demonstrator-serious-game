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
        [Key] public string Id { get; set; }
        public Phase CurrentPhase { get; set; }
        public int CurrentDay { get; set; }
        private Player _retailer;

        public Player Retailer
        {
            get { return _retailer; }
            set
            {
                if (value != null)
                {
                    if (value.Role.Id != "Retailer")
                        throw new ArgumentException("Given role id does not match the expected role Retailer");
                    Players.Add(value);
                    _retailer = value;
                }
            }
        }

        private Player _manufacturer;

        public Player Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                if (value != null)
                {
                    if (value.Role.Id != "Manufacturer")
                        throw new ArgumentException("Given role id does not match the expected role Manufacturer");
                    Players.Add(value);
                    _manufacturer = value;
                }
            }
        }

        private Player _processor;

        public Player Processor
        {
            get { return _processor; }
            set
            {
                if (value != null)
                {
                    if (value.Role.Id != "Processor")
                        throw new ArgumentException("Given role id does not match the expected role Processor");
                    Players.Add(value);
                    _processor = value;
                }
            }
        }

        private Player _farmer;

        public Player Farmer
        {
            get { return _farmer; }
            set
            {
                if (value != null)
                {
                    if (value.Role.Id != "Farmer")
                        throw new ArgumentException("Given role id does not match the expected role Farmer");
                    Players.Add(value);
                    _farmer = value;
                }
            }
        }

        //TODO: has bug where it is initialized twice, once during getting from database and second when serialized in web controller
        [NotMapped] public List<Player> Players { get; set; }
        public bool GameStarted { get; set; }


        public HashSet<int> IdList = new HashSet<int>();

        public Game()
        {
            Players = new List<Player>();
            Id = CreateUniqueId();
            CurrentPhase = Phase.Phase1;
            CurrentDay = 1;
            GameStarted = false;
        }

        /**
         * <summary>Creates a unique id using six numbers</summary>
         * <returns>Unique id as string</returns>
         * <remarks>For now it returns a string later on, we might need to change that to an integer</remarks>
         */
        private String CreateUniqueId()
        {
            Random r = new Random();
            int Id;
            while (true)
            {
                Id = r.Next(100000, 1000000);

                if (!IdList.Contains(Id))
                {
                    IdList.Add(Id);
                    String IdS = Id.ToString();
                    return IdS;
                }
            }
        }

        /**
         * <summary>Makes game Progress to next round</summary>
         */
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

                SendPayments();
                SetHoldingCosts();
                UpdateBalance();

                SendOrders();
                CurrentDay += Factors.RoundIncrement;
            }
        }

        /**
         * <summary>Adds default order to each actor</summary>
         * <remarks>Only needs to be used at the start of each game</remarks>
         */
        private void SetSetupOrders()
        {
            Retailer.IncomingOrders.Add(new Order
                {OrderDay = 1 - Factors.RoundIncrement, Volume = 5, Price = Factors.ManuProductPrice * 10});
            Manufacturer.IncomingOrders.Add(new Order
                {OrderDay = 1 - Factors.RoundIncrement, Volume = 5, Price = Factors.ProcProductPrice * 10});
            Processor.IncomingOrders.Add(new Order
                {OrderDay = 1 - Factors.RoundIncrement, Volume = 5, Price = Factors.FarmerProductPrice * 10});
            Farmer.IncomingOrders.Add(new Order
                {OrderDay = 1 - Factors.RoundIncrement, Volume = 5, Price = Factors.HarvesterProductPrice * 10});
        }

        /**
         * <summary>Adds default deliveries to each actor</summary>
         * <remarks>Only needs to be used at the start of each game</remarks>
         */
        private void SetSetupDeliveries() //TODO: check math ceiling stuff
        {
            for (int i = 0; i < (int) Math.Ceiling(Manufacturer.Role.LeadTime / (double) Factors.RoundIncrement); i++)
            {
                Retailer.IncomingDeliveries.Add(new Order()
                    {Volume = 5, ArrivalDay = Factors.RoundIncrement * i + 1, Price = Factors.ManuProductPrice * 5});
            }

            for (int i = 0; i < (int) Math.Ceiling(Processor.Role.LeadTime / (double) Factors.RoundIncrement); i++)
            {
                Manufacturer.IncomingDeliveries.Add(new Order()
                    {Volume = 5, ArrivalDay = Factors.RoundIncrement * i + 1, Price = Factors.ProcProductPrice * 5});
            }

            for (int i = 0; i < (int) Math.Ceiling(Farmer.Role.LeadTime / (double) Factors.RoundIncrement); i++)
            {
                Processor.IncomingDeliveries.Add(new Order()
                    {Volume = 5, ArrivalDay = Factors.RoundIncrement * i + 1, Price = Factors.FarmerProductPrice * 5});
            }

            for (int i = 0; i < (int) Math.Ceiling(1 / (double) Factors.RoundIncrement); i++)
            {
                Farmer.IncomingDeliveries.Add(new Order()
                    {Volume = 5, ArrivalDay = Factors.RoundIncrement * i + 1, Price = Factors.HarvesterProductPrice * 5});
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

        /**
         * <summary>Sets IncomingOrder for every actor</summary>
         */
        private void SendOrders()
        {
            AddingCurrentCDay();
            AddingOrderNumber();
            AddingPrice();
            AddOrder();
            //AddOrderToHistory();
        }

        /**
         * <summary>Adds current day to each actors current order</summary>
         */
        public void AddingCurrentCDay()
        {
            // Adding current day
            Retailer.CurrentOrder.OrderDay = CurrentDay;
            Manufacturer.CurrentOrder.OrderDay = CurrentDay;
            Processor.CurrentOrder.OrderDay = CurrentDay;
            Farmer.CurrentOrder.OrderDay = CurrentDay;
        }

        /**
         * <summary>Adds order number to each actors current order</summary>
         */
        public void AddingOrderNumber()
        {
            // Adding order number
            Retailer.CurrentOrder.OrderNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(CurrentDay) / 5));
            Manufacturer.CurrentOrder.OrderNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(CurrentDay) / 5));
            Processor.CurrentOrder.OrderNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(CurrentDay) / 5));
            Farmer.CurrentOrder.OrderNumber = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(CurrentDay) / 5));
        }

        /**
         * <summary>Adds the price of the order to each actors current order</summary>
         */
        public void AddingPrice()
        {
            // Adding Price
            Retailer.CurrentOrder.Price = Factors.ManuProductPrice * Retailer.CurrentOrder.Volume;
            Manufacturer.CurrentOrder.Price = Factors.ProcProductPrice * Manufacturer.CurrentOrder.Volume;
            Processor.CurrentOrder.Price = Factors.FarmerProductPrice * Processor.CurrentOrder.Volume;
            //TODO: implement better way for farmer to place orders
            Farmer.CurrentOrder.Price = Factors.HarvesterProductPrice * Farmer.CurrentOrder.Volume;
        }

        /**
         * <summary>Adds current order to each actors supplier</summary>
         */
        public void AddOrder()
        {
            // Making new order
            //TODO: Volume might not equal to volume in payment amount
            Retailer.IncomingOrders.Add(new Order() {OrderDay = CurrentDay, Volume = new Random().Next(5, 15)});
            Manufacturer.IncomingOrders.Add(Retailer.CurrentOrder);
            Processor.IncomingOrders.Add(Manufacturer.CurrentOrder);
            Farmer.IncomingOrders.Add(Processor.CurrentOrder);
        }

        /**
         * <summary>Adds order history to each actors</summary>
         */
        public void AddOrderToHistory()
        {
            // Add order to history
            Retailer.OrderHistory.Add(new Order()
                {OrderNumber = Retailer.CurrentOrder.OrderNumber, Volume = Retailer.CurrentOrder.Volume});
            Manufacturer.OrderHistory.Add(new Order()
                {OrderNumber = Manufacturer.CurrentOrder.OrderNumber, Volume = Manufacturer.CurrentOrder.Volume});
            Processor.OrderHistory.Add(new Order()
                {OrderNumber = Processor.CurrentOrder.OrderNumber, Volume = Processor.CurrentOrder.Volume});
            Farmer.OrderHistory.Add(new Order()
                {OrderNumber = Farmer.CurrentOrder.OrderNumber, Volume = Farmer.CurrentOrder.Volume});
        }

        /**
         * <summary>Adds Payment(s) to each supplier Payments list, so they get paid for their delivered goods</summary>
         */
        private void SendPayments()
        {
            int customerOrderVolume = new Random().Next(5, 15);
            Manufacturer.Payments.AddRange(Retailer.GetOutgoingPayments(CurrentDay, Manufacturer.Id));
            Processor.Payments.AddRange(Manufacturer.GetOutgoingPayments(CurrentDay, Processor.Id));
            Farmer.Payments.AddRange(Processor.GetOutgoingPayments(CurrentDay, Farmer.Id));
            //TODO: Change this, now the retailer will get paid every round even though he might not have fulfilled an order
            //TODO: The amount might be different to the actual ordered amount, this needs to change
            Retailer.Payments.Add(new Payment()
            {
                Amount = customerOrderVolume * Factors.RetailProductPrice, DueDay = CurrentDay + 2, FromPlayer = true,
                PlayerId = Retailer.Id, Id = Guid.NewGuid().ToString()
            });
        }

        /**
         * <summary>Adds outgoing deliveries to the IncomingDelivery list</summary>
         */
        private void SendDeliveries()
        {
            Retailer.GetOutgoingDeliveries(CurrentDay);
            Retailer.IncomingDeliveries.AddRange(Manufacturer.GetOutgoingDeliveries(CurrentDay));
            Manufacturer.IncomingDeliveries.AddRange(Processor.GetOutgoingDeliveries(CurrentDay));
            Processor.IncomingDeliveries.AddRange(Farmer.GetOutgoingDeliveries(CurrentDay));
            //TODO: Implement later
            Farmer.IncomingDeliveries.Add(new Order()
            {
                OrderDay = CurrentDay, ArrivalDay = CurrentDay + new Random().Next(3, 6),
                Volume = Farmer.CurrentOrder.Volume
            });
        }

        /**
         * <summary>Causes each actor to process their deliveries</summary>
         */
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

        /**
         * <summary>Adds a standard payment for the setup costs to each actors payment list</summary>
         * <remarks>Only needs to be called once, at the start of the game</remarks>
         */
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
                    Amount = Factors.SetupCost * -1, DueDay = 1, FromPlayer = false, PlayerId = player.Id,
                    Id = Guid.NewGuid().ToString()
                });
            }
        }

        /**
         * <summary>Calls the UpdateBalance method for each player</summary>
         */
        private void UpdateBalance()
        {
            foreach (Player player in Players)
            {
                player.UpdateBalance(CurrentDay);
            }
        }

        /**
         * <summary>Adds holding cost to each players Payments list</summary>
         */
        private void SetHoldingCosts()
        {
            foreach (Player player in Players)
            {
                player.SetHoldingCost(CurrentDay);
            }
        }
    }
}