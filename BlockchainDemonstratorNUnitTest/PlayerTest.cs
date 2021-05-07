using BlockchainDemonstratorApi.Models.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainDemonstratorNUnitTest
{
    [TestFixture]
    class PlayerTests
    {
        private Player _player;


        [SetUp]
        public void Setup()
        {
            _player = new Player("Test");
        }


        /*[TestCase(20, 20, 16, 20)]
        [TestCase(30, 10, 45, 30)]
        [TestCase(50, 5, 23, 28)]
        [TestCase(100, 100, 100, 100)]
        [TestCase(50, 25, 10, 35)]
        public void ShipmentTest_InventoryIsSmallerThanBackorder(int inventory, int backorder, int volume, int result) TODO(Shaun): Sorry has to be rewritten, I changed how the backorder and incomingorders worked.
        {
            _player.Inventory = inventory;
            _player.Backorder = backorder;
            _player.IncomingOrders = new Order() { Volume = volume };

            Assert.AreEqual(_player.GetOutgoingVolume(), result);
        }*/



        public void SendDeliveryTest()
        {




        }





    }
}
