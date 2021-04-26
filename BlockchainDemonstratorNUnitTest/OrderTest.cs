using System;
using BlockchainDemonstratorApi.Models.Classes;
using NUnit.Framework;

namespace BlockchainDemonstratorNUnitTest
{
    [TestFixture]
    public class OrderTest
    {
        private Order _order;
        
        [SetUp]
        public void Setup()
        {
            _order = new Order();
        }

        [TestCase(-1)]
        [TestCase(-20)]
        [TestCase(-618)]
        public void When_OrderVolumeLowerThan0_Expect0(int volume)
        {

            Assert.Throws<ArgumentException>(() => { _order.Volume = volume; });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(25)]
        [TestCase(94)]
        [TestCase(678)]
        public void When_OrderVolumeGreaterOrEqualTo0_ExpectVolumeToBeValue(int volume)
        {
            _order.Volume = volume;

            Assert.AreEqual(_order.Volume, volume);
        }
    }
}