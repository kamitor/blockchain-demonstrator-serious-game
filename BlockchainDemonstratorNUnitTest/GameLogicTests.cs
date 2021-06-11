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
        
        [Test]
        public void ProgressIncomingOrdersTests()
        {
            _game.Progress();
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(_game.Retailer.IncomingOrders[0].Volume >= 5 && _game.Retailer.IncomingOrders[0].Volume <= 15);
                Assert.IsTrue(_game.Manufacturer.IncomingOrders[0].Volume == 10);
                Assert.IsTrue(_game.Processor.IncomingOrders[0].Volume >= 11);
                Assert.IsTrue(_game.Farmer.IncomingOrders[0].Volume >= 12);
            });
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
        [Repeat(25)]
        public void ProgressIncomingDeliveries()
        {
            _game.Progress();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(_game.Retailer.OutgoingOrders.Find(o => o.Volume == 10) != null);
                Assert.IsTrue(_game.Manufacturer.OutgoingOrders.Find(o => o.Volume == 11) != null);
                Assert.IsTrue(_game.Processor.OutgoingOrders.Find(o => o.Volume == 12) != null);
                Assert.IsTrue(_game.Farmer.OutgoingOrders.Find(o => o.Volume == 13) != null);
            });
        }

        [Test]
        [Repeat(25)]
        public void ProgressProcessDeliveries()
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

            _game.Progress();
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(_game.Retailer.Inventory != retailerInventory);
                Assert.IsTrue(_game.Manufacturer.Inventory != manufacturerInventory);
                Assert.IsTrue(_game.Processor.Inventory != processorInventory);
                Assert.IsTrue(_game.Farmer.Inventory != farmerInventory);
            });
        }

        [Test]
        public void OrderFailedToDeliverFullVolume_ExcessAddedToBackorder() 
        {
            _game.Manufacturer.Inventory = 10;
            _game.Manufacturer.IncomingOrders.Add(new Order() {Volume = 20, OrderDay = -1});
            
            _game.Manufacturer.GetOutgoingDeliveries(1);

            int result = _game.Manufacturer.Backorder;
            
            Assert.AreEqual(10, result);
        }
        
        [Test]
        public void OrderPriceSubtractedFromBalance_expectTrue()
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
        
        [Test]
        [Repeat(25)]
        public void OrderPriceAddedToBalance_expectTrue() 
        {
            
            _game.Manufacturer.IncomingOrders.Add(new Order() {Volume = 10});
            
            _game.Manufacturer.GetOutgoingDeliveries(1);
            
            _game.Manufacturer.UpdateBalance(26);

            int expected = Factors.ManuProductPrice * 10;
            Assert.AreEqual(expected, _game.Manufacturer.Balance);
            
        }
    }
}
