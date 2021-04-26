using BlockchainDemonstratorApi.Models.Classes;
using NUnit.Framework;

namespace BlockchainDemonstratorNUnitTest
{
    [TestFixture]
    public class GameTests
    {
        private Game _game;

        [SetUp]
        public void Setup()
        {
            _game = new Game();
        }

        [Test]
        public void JoinGameTest()
        {
            Player player = new Player("FarmerTest");
            _game.Farmer = player;
            Assert.AreEqual(player, _game.Farmer);
        }

        [Test]
        public void JoinNullGameTest()
        {
            Player player = new Player("FarmerTest");
            _game.Farmer = player;
            _game.Farmer = null;
            Assert.AreEqual(player, _game.Farmer);
        }
    }
}