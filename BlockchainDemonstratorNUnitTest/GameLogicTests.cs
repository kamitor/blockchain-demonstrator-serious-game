using BlockchainDemonstratorApi.Models.Classes;
using BlockchainDemonstratorApi.Models.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainDemonstratorNUnitTest
{
    [TestFixture]
    public class GameLogicTests
    {
        private Game _game;

        [SetUp]
        public void SetUp()
        {
            _game = new Game();


            Player retailer = new Player("RetailerTest");
            retailer.Role = new Role("Retailer", 1.7083333, Product.Beer);
            _game.Retailer = retailer;

            Player manufacturer = new Player("ManufacturerTest");
            manufacturer.Role = new Role("Manufacturer", 1.375, Product.Packs);
            _game.Manufacturer = manufacturer;

            Player processor = new Player("ProcessorTest");
            processor.Role = new Role("Processor", 17.166667, Product.Barley);
            _game.Processor = processor;

            Player farmer = new Player("FarmerTest");
            farmer.Role = new Role("Farmer", 22.333333, Product.Seeds);
            _game.Farmer = farmer;

            _game.Retailer.CurrentOrder = new Order { Volume = 10 };
            _game.Manufacturer.CurrentOrder = new Order { Volume = 11 };
            _game.Processor.CurrentOrder = new Order { Volume = 12 };
            _game.Farmer.CurrentOrder = new Order { Volume = 13 };
        }

        /*[Test]
        public void ProgressIncomingOrdersTests() //TODO: rewrite later
        {
            _game.Progress();

            if(_game.Retailer.IncomingOrders.OrderDay == 1 && _game.Retailer.IncomingOrders.Volume >= 5 && _game.Retailer.IncomingOrders.Volume <= 15 &&
                _game.Manufacturer.IncomingOrders.OrderDay == 1 && _game.Manufacturer.IncomingOrders.Volume == 10 &&
                _game.Processor.IncomingOrders.OrderDay == 1 && _game.Processor.IncomingOrders.Volume == 11 &&
                _game.Farmer.IncomingOrders.OrderDay == 1 && _game.Farmer.IncomingOrders.Volume == 12)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }*/

        [Test]
        public void ProgressInventoriesTest()
        {
            _game.Progress();

            if (_game.Retailer.Inventory >= 5 && _game.Retailer.Inventory <= 15 &&
                _game.Manufacturer.Inventory == 10 &&
                _game.Processor.Inventory == 9 &&
                _game.Farmer.Inventory == 8)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void ProgressIncomingDeliveries()
        {
            _game.Progress();

            if (_game.Retailer.IncomingDeliveries.Find(o => o.OrderDay == 1 && o.Volume == 10) != null &&
                _game.Manufacturer.IncomingDeliveries.Find(o => o.OrderDay == 1 && o.Volume == 11) != null &&
                _game.Processor.IncomingDeliveries.Find(o => o.OrderDay == 1 && o.Volume == 12) != null &&
                _game.Farmer.IncomingDeliveries.Find(o => o.OrderDay == 1 && o.Volume == 13) != null)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

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
    }
}
