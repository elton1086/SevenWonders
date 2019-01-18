using AutoFixture;
using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Factories;
using SevenWonders.Services;
using SevenWonders.XUnit.Test.AutoData;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SevenWonders.UnitTest.Services
{
    public class TradeManagerTest
    {
        [Theory, AutoMoqData]
        public void SetupCoinsFromBankTest(List<GamePlayer> players, TradeManager manager)
        {
            manager.SetupCoinsFromBank(players);
            Assert.All(players, p => Assert.Equal(3, p.Coins));
        }

        [Theory, AutoGameSetupData(5)]
        public void ResolveMilitaryConflictsForAgeOneTest(TradeManager manager, List<GamePlayer> players)
        {
            var cards = CreateCards();
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[3].Cards.Add(cards.First(c => c.Name == CardName.Stockade));

            manager.ResolveMilitaryConflicts(players, Age.I);

            Assert.Equal(0, players[0].ConflictTokenSum);
            Assert.Equal(2, players[1].ConflictTokenSum);
            Assert.Equal(-1, players[2].ConflictTokenSum);
            Assert.Equal(1, players[3].ConflictTokenSum);
            Assert.Equal(-2, players[4].ConflictTokenSum);
        }

        [Theory, AutoGameSetupData(5)]
        public void ResolveMilitaryConflictsForAgeTwoTest(TradeManager manager, List<GamePlayer> players)
        {
            var cards = CreateCards();
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[3].Cards.Add(cards.First(c => c.Name == CardName.Stockade));

            players[0].Cards.Add(cards.First(c => c.Name == CardName.Stables));
            players[4].Cards.Add(cards.First(c => c.Name == CardName.Stables));

            manager.ResolveMilitaryConflicts(players, Age.II);

            Assert.Equal(6, players[0].ConflictTokenSum);
            Assert.Equal(2, players[1].ConflictTokenSum);
            Assert.Equal(-1, players[2].ConflictTokenSum);
            Assert.Equal(-1, players[3].ConflictTokenSum);
            Assert.Equal(2, players[4].ConflictTokenSum);
        }

        [Theory, AutoGameSetupData(5)]
        public void ResolveMilitaryConflictsForAgeThreeWithWonderTest(TradeManager manager, List<GamePlayer> players)
        {
            var cards = CreateCards();
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[3].Cards.Add(cards.First(c => c.Name == CardName.Stockade));

            players[0].Cards.Add(cards.First(c => c.Name == CardName.Stables));
            players[4].Cards.Add(cards.First(c => c.Name == CardName.Stables));

            players[1].Cards.Add(cards.First(c => c.Name == CardName.Circus));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Circus));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Circus));
            players[3].Cards.Add(cards.First(c => c.Name == CardName.Circus));

            players[0].SetWonder(WonderFactory.CreateWonder(WonderName.ColossusOfRhodes, WonderBoardSide.B));
            players[0].Wonder.BuildStage();
            players[0].Wonder.BuildStage();

            manager.ResolveMilitaryConflicts(players, Age.III);

            Assert.Equal(5, players[0].ConflictTokenSum);
            Assert.Equal(-1, players[1].ConflictTokenSum);
            Assert.Equal(10, players[2].ConflictTokenSum);
            Assert.Equal(4, players[3].ConflictTokenSum);
            Assert.Equal(-2, players[4].ConflictTokenSum);
        }

        private IEnumerable<StructureCard> CreateCards()
        {
            return new List<StructureCard>
            {
                new MilitaryCard(CardName.Stockade, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.Shield, 1) }),
                new MilitaryCard(CardName.Stables, 3, Age.II, null, null, new List<Effect> { new Effect(EffectType.Shield, 2) }),
                new MilitaryCard(CardName.Circus, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Shield, 3) }),
            };
        }
    }
}
