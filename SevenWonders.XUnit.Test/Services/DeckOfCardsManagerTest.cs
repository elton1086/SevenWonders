using SevenWonders.BaseEntities;
using SevenWonders.Services;
using SevenWonders.XUnit.Test.AutoData;
using System;
using System.Linq;
using Xunit;

namespace SevenWonders.UnitTest.Services
{
    public class DeckOfCardsManagerTest
    {
        [Theory, AutoMoqData]
        public void GetShuffledWonderCardsTest(DeckOfCardsManager manager)
        {
            var result = manager.GetShuffledWonderCards();
            Assert.Equal(Enum.GetValues(typeof(WonderName)).Length, result.Count);
        }

        [Theory]
        [AutoMoqInlineData(5)]
        [AutoMoqInlineData(7)]
        public void GetShuffledDeckForNPlayers(int numberOfPlayers, DeckOfCardsManager manager)
        {
            var result = manager.GetShuffledDeck(numberOfPlayers);
            Assert.Equal(numberOfPlayers * 7 * 3, result.Count);
            Assert.Equal(numberOfPlayers + 2, result.Count(x => x.Type == StructureType.Guilds));
            Assert.Equal(0, result.Count(x => x.PlayersCount > numberOfPlayers));
        }
    }
}
