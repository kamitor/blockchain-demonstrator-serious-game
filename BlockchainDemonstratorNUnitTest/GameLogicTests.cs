using BlockchainDemonstratorApi.Models.Classes;
using BlockchainDemonstratorApi.Models.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using NuGet.Frameworks;

namespace BlockchainDemonstratorNUnitTest
{
    [TestFixture]
    public class GameLogicTests
    {
        private Game _game;

        [SetUp]
        public void SetUp()
        {
            _game = new Game("123456");


            Player retailer = new Player("RetailerTest");
            retailer.Role = new Role("Retailer", 1.7083333, Product.Beer);
            retailer.ChosenOption = new Option("Basic", 75000, 3500, 710, 516, 1.375, 0, 750);
            _game.Retailer = retailer;

            Player manufacturer = new Player("ManufacturerTest");
            manufacturer.Role = new Role("Manufacturer", 1.375, Product.Packs);
            manufacturer.ChosenOption = new Option("Basic", 75000, 3500, 710, 516, 1.375, 0, 750);
            _game.Manufacturer = manufacturer;

            Player processor = new Player("ProcessorTest");
            processor.Role = new Role("Processor", 17.166667, Product.Barley);
            processor.ChosenOption = new Option("Basic", 75000, 3500, 710, 516, 1.375, 0, 750);
            _game.Processor = processor;

            Player farmer = new Player("FarmerTest");
            farmer.Role = new Role("Farmer", 22.333333, Product.Seeds);
            farmer.ChosenOption = new Option("Basic", 75000, 3500, 710, 516, 1.375, 0, 750);
            _game.Farmer = farmer;

            _game.Retailer.CurrentOrder = new Order { Volume = 10 };
            _game.Manufacturer.CurrentOrder = new Order { Volume = 11 };
            _game.Processor.CurrentOrder = new Order { Volume = 12 };
            _game.Farmer.CurrentOrder = new Order { Volume = 13 };
        }
/*
        //TODO: fix test or delete
        [Test]
        public void ProgressIncomingOrdersTests()
        {
            _game.Progress();

            // if(_game.Retailer.IncomingOrders.OrderDay == 1 && _game.Retailer.IncomingOrders.Volume >= 5 && _game.Retailer.IncomingOrders.Volume <= 15 &&
            //     _game.Manufacturer.IncomingOrders.OrderDay == 1 && _game.Manufacturer.IncomingOrders.Volume == 10 &&
            //     _game.Processor.IncomingOrders.OrderDay == 1 && _game.Processor.IncomingOrders.Volume == 11 &&
            //     _game.Farmer.IncomingOrders.OrderDay == 1 && _game.Farmer.IncomingOrders.Volume == 12)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void ProgressInventoriesTest()
        {
            _game.Progress();
            _game.Progress();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(_game.Retailer.Inventory >= 5 && _game.Retailer.Inventory <= 15);
                Assert.AreEqual(10, _game.Manufacturer.Inventory);
                Assert.AreEqual(9, _game.Processor.Inventory);
                Assert.AreEqual(8, _game.Farmer.Inventory);
            });
        }

        [Test]
        public void ProgressIncomingDeliveries()
        {
            _game.Progress();

            if (_game.Retailer.OutgoingOrders.Find(o => o.OrderDay == 1 && o.Volume == 10) != null &&
                _game.Manufacturer.OutgoingOrders.Find(o => o.OrderDay == 1 && o.Volume == 11) != null &&
                _game.Processor.OutgoingOrders.Find(o => o.OrderDay == 1 && o.Volume == 12) != null &&
                _game.Farmer.OutgoingOrders.Find(o => o.OrderDay == 1 && o.Volume == 13) != null)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        //TODO: fix test use repeat function
        [Test]
        public void ProgressProcessDeliveries() //TODO: Does not always work even though 5*10 = 50 days is more than maximum possible leadtime
        {
            _game.Progress();

            _game.Retailer.CurrentOrder = new Order() {Volume = 0 };
            _game.Manufacturer.CurrentOrder = new Order() { Volume = 0 };
            _game.Processor.CurrentOrder = new Order() { Volume = 0 };
            _game.Farmer.CurrentOrder = new Order() { Volume = 0 };

            int retailerInventory = _game.Retailer.Inventory;
            int manufacturerInventory = _game.Manufacturer.Inventory;
            int processorInventory = _game.Processor.Inventory;
            int farmerInventory = _game.Farmer.Inventory;

            for (int i = 0; i < 10; i++)
            {
                _game.Progress();
                if(_game.Retailer.Inventory != retailerInventory &&
                    _game.Manufacturer.Inventory != manufacturerInventory &&
                    _game.Processor.Inventory != processorInventory &&
                    _game.Farmer.Inventory != farmerInventory)
                {
                    Assert.Pass();
                }
            }

            Assert.Fail();
        }

        [Test]
        public void EachActorHasDifferentLeadtime_True()
        {
            int leadtimeRetailer = (int)_game.Retailer.Role.LeadTime;
            int leadtimeManufacturer = (int)_game.Manufacturer.Role.LeadTime;
            int leadtimeProcessor = (int)_game.Processor.Role.LeadTime;
            int leadtimeFarmer = (int)_game.Farmer.Role.LeadTime;
            bool result = false;

            if (leadtimeRetailer != leadtimeManufacturer && leadtimeRetailer != leadtimeProcessor && leadtimeRetailer != leadtimeFarmer)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            if (leadtimeManufacturer != leadtimeProcessor && leadtimeManufacturer != leadtimeFarmer)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            if (leadtimeProcessor != leadtimeFarmer)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            Assert.IsTrue(result);
        }

        //TODO: fix test or delete
        [Test]
        public void OrderLeadtimeRandomlyIncreases() //TODO: No longer works because GetOutgoingDeliveries() is a void
        {
            _game.Manufacturer.Inventory = 20;
            _game.Manufacturer.IncomingOrders.Add(new Order() {Volume = 20});
            
            /*List<Order> result = _game.Manufacturer.GetOutgoingDeliveries(1);
            
            if (result.First().ArrivalDay >= _game.Manufacturer.Role.LeadTime + 1)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void OrderFailedToDeliverFullVolume_ExcessAddedToBackorder() //TODO: Probably no longer works because of order rework
        {
            _game.Manufacturer.Inventory = 10;
            _game.Manufacturer.IncomingOrders.Add(new Order() {Volume = 20, OrderDay = -1});
            

            _game.Manufacturer.GetOutgoingDeliveries(1);

            int result = _game.Manufacturer.Backorder;
            
            Assert.AreEqual(10, result);
        }

        [Test]
        public void OrderPriceSubtractedFromBalance_expectTrue() //No longer works because order does not have price
        {
            List<Delivery> deliveries = new List<Delivery>();
            deliveries.Add(new Delivery()
            {
                Volume = 20,
                Price = 2000
            });
            
            Order order = new Order()
            {
                Volume = 20,
                Deliveries = deliveries
            };
            
            _game.Manufacturer.OutgoingOrders.Add(order);
            
            _game.Manufacturer.ProcessDeliveries(1);
            _game.Manufacturer.UpdateBalance(8);
            
            Assert.AreEqual(-2000, _game.Manufacturer.Balance);
        }
        
        //TODO: fix test or delete
        [Test]
        [Repeat(25)]
        public void OrderPriceAddedToBalance_expectTrue() //No longer works because order does not have price
        {
            
            _game.Manufacturer.IncomingOrders.Add(new Order() {Volume = 10});
            
            _game.Manufacturer.GetOutgoingDeliveries(1);
            
            _game.Manufacturer.UpdateBalance(26);

            int expected = Factors.ManuProductPrice * 10;
            Assert.AreEqual(expected, _game.Manufacturer.Balance);
            
        } */
    }
}
