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

        [Test]
        public void ProgressGameTest()
        {
            _game.Progress();

            if (_game.Retailer.Inventory < )

                Assert.Pass();
        }
    }
}
