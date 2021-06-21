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
    }
}
