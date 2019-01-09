using AutoFixture;
using AutoFixture.Xunit2;
using Moq;
using SevenWonders.BaseEntities;
using SevenWonders.CardGenerator;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Factories;
using SevenWonders.Services;
using SevenWonders.Services.Contracts;
using SevenWonders.XUnit.Test.AutoData;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SevenWonders.UnitTest
{
    public class GameFlowManagerTest
    {
        [Theory, AutoMoqData]
        public void SetupGameFor5PlayersTest([Frozen]IFixture fixture, [Frozen]Mock<IDeckOfCardsManager> deckManager,
            GameFlowManager manager)
        {
            deckManager.Setup(m => m.GetShuffledWonderCards())
                .Returns(fixture.CreateMany<WonderCard>(7).ToList());

            manager.CreateNewPlayer("maria");
            manager.CreateNewPlayer("joão");
            manager.CreateNewPlayer("josé");
            manager.CreateNewPlayer("ana");
            manager.CreateNewPlayer("pedro");
            manager.SetupGame();

            Assert.Equal(5, manager.Players.Count);
            //Base game has 7 wonders - number of players
            Assert.Equal(2, manager.WonderCards.Count);
            Assert.NotEqual(manager.Players[1].Wonder.Name, manager.Players[3].Wonder.Name);
        }

        [Theory, AutoMoqData]
        public void DealAgeCardsFor3PlayersTest([Frozen]Mock<IDeckOfCardsManager> deckManager,
            GameFlowManager manager)
        {
            deckManager.Setup(m => m.GetShuffledDeck(3))
                .Returns(CardMappingGenerator.GenerateBaseGameCards().CardMapping
                    .Take(21)
                    .Select(c => StructureCardFactory.CreateStructureCard(c))
                    .ToList());

            manager.CreateNewPlayer("paul");
            manager.CreateNewPlayer("mary");
            manager.CreateNewPlayer("tracy");
            manager.SetupGame();
            manager.StartAge();
            Assert.All(manager.Players, p => Assert.Equal(7, p.SelectableCards.Count));
            Assert.All(manager.Players, p => Assert.All(p.SelectableCards, c => Assert.Equal(Age.I, c.Age)));
        }        
    }
}
