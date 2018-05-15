using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenWonder.BaseEntities;
using SevenWonder.Services;
using SevenWonder.Utilities;
using System;
using System.Linq;

namespace SevenWonders.UnitTest.Services
{
    [TestClass]
    public class DeckOfCardsManagerTest
    {
        DeckOfCardsManager manager;
 
        [TestInitialize]
        public void Initialize()
        {
            manager = new DeckOfCardsManager(new NullUnitOfWork());
        }

        [TestMethod]
        public void GetShuffledWonderCardsTest()
        {
            var result = manager.GetShuffledWonderCards();
            Assert.AreEqual(Enum.GetValues(typeof(WonderName)).Length, result.Count);
        }

        [TestMethod]
        public void GetShuffledDeckFor5Players()
        {
            var numberOfPlayers = 5;
            var result = manager.GetShuffledDeck(numberOfPlayers);
            Assert.AreEqual(result.Count, numberOfPlayers * 7 * 3);
            Assert.AreEqual(result.Count(x => x.Type == StructureType.Guilds), numberOfPlayers + 2);
            Assert.AreEqual(result.Count(x => x.PlayersCount > numberOfPlayers), 0);
        }

        [TestMethod]
        public void GetShuffledDeckFor7Players()
        {
            var numberOfPlayers = 7;
            var result = manager.GetShuffledDeck(numberOfPlayers);
            Assert.AreEqual(result.Count, numberOfPlayers * 7 * 3);
            Assert.AreEqual(result.Count(x => x.Type == StructureType.Guilds), numberOfPlayers + 2);
        }
    }
}
