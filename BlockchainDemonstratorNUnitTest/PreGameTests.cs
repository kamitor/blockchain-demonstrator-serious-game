using BlockchainDemonstratorApi.Models.Classes;
using BlockchainDemonstratorApi.Models.Enums;
using NUnit.Framework;
using System;

namespace BlockchainDemonstratorNUnitTest
{
    [TestFixture]
    public class PreGameTests
    {
        private Game _game;

        [SetUp]
        public void Setup()
        {
            _game = new Game("123456");
        }

        [Test]
        public void JoinGameTest()
        {
            Player player = new Player("FarmerTest");
            player.Role = new Role("Farmer", 10, Product.Seeds);
            _game.Farmer = player;
            Assert.AreEqual(player, _game.Farmer);
        }

        [Test]
        public void JoinWrongRoleTest()
        {
            Player player = new Player("FarmerTest");
            player.Role = new Role("Retailer", 10, Product.Seeds);
            Assert.Throws<ArgumentException>(() => { _game.Farmer = player; });
        }

        [Test]
        public void PlayersListTest()
        {
            Player farmer = new Player("FarmerTest");
            farmer.Role = new Role("Farmer", 10, Product.Seeds);
            _game.Farmer = farmer;

            Player retailer = new Player("RetailerTest");
            retailer.Role = new Role("Retailer", 10, Product.Seeds);
            _game.Retailer = retailer;

            Assert.AreEqual(2, _game.Players.Count);
        }

        
    }
}