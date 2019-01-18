using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Factories;
using SevenWonders.Services.VictoryPoints;
using SevenWonders.XUnit.Test.AutoData;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SevenWonders.UnitTest.Services
{
    public class ScientificStructuresCategoryTest
    {
        [Fact]
        public void GetBestCombinationOneSymbolTest()
        {
            var input = new List<ScientificSymbol> { ScientificSymbol.Gear };
            var output1 = ScientificStructuresCategory.GetBestCombination(input, 1);

            var output2 = ScientificStructuresCategory.GetBestCombination(input, 2);

            Assert.Equal(2, output1.Count(s => s == ScientificSymbol.Gear));

            Assert.Equal(1, output2.Count(s => s == ScientificSymbol.Gear));
            Assert.Equal(1, output2.Count(s => s == ScientificSymbol.Tablet));
            Assert.Equal(1, output2.Count(s => s == ScientificSymbol.Compass));
        }

        [Fact]
        public void GetBestCombinationTwoDifferentSymbolsTest()
        {
            var input = new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Tablet };
            var output1 = ScientificStructuresCategory.GetBestCombination(input, 1);

            Assert.Equal(1, output1.Count(s => s == ScientificSymbol.Gear));
            Assert.Equal(1, output1.Count(s => s == ScientificSymbol.Tablet));
            Assert.Equal(1, output1.Count(s => s == ScientificSymbol.Compass));
        }

        [Fact]
        public void GetBestCombinationTwoSameSymbolsTest()
        {
            var input = new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Gear };
            var output1 = ScientificStructuresCategory.GetBestCombination(input, 1);

            var output2 = ScientificStructuresCategory.GetBestCombination(input, 2);

            Assert.Equal(3, output1.Count(s => s == ScientificSymbol.Gear));

            Assert.Equal(4, output2.Count(s => s == ScientificSymbol.Gear));
        }

        [Fact]
        public void GetBestCombinationThreeDifferentSymbolsTest()
        {
            var input = new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Compass, ScientificSymbol.Tablet };
            var output1 = ScientificStructuresCategory.GetBestCombination(input, 2);

            var output2 = ScientificStructuresCategory.GetBestCombination(input, 3);

            Assert.Equal(3, output1.Count(s => s == ScientificSymbol.Compass));

            Assert.Equal(2, output2.Count(s => s == ScientificSymbol.Compass));
            Assert.Equal(2, output2.Count(s => s == ScientificSymbol.Gear));
            Assert.Equal(2, output2.Count(s => s == ScientificSymbol.Tablet));
        }

        [Fact]
        public void GetBestCombinationThreeSameSymbolsTest()
        {
            var input = new List<ScientificSymbol> { ScientificSymbol.Compass, ScientificSymbol.Compass, ScientificSymbol.Compass };
            var output1 = ScientificStructuresCategory.GetBestCombination(input, 2);

            Assert.Equal(5, output1.Count(s => s == ScientificSymbol.Compass));
        }

        [Fact]
        public void GetBestCombinationTwoAndOneAndOneSymbolsTest()
        {
            var input = new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Gear, ScientificSymbol.Tablet, ScientificSymbol.Compass };
            var output1 = ScientificStructuresCategory.GetBestCombination(input, 2);

            var output2 = ScientificStructuresCategory.GetBestCombination(input, 3);

            Assert.Equal(2, output1.Count(s => s == ScientificSymbol.Compass));
            Assert.Equal(2, output1.Count(s => s == ScientificSymbol.Gear));
            Assert.Equal(2, output1.Count(s => s == ScientificSymbol.Tablet));

            Assert.Equal(5, output2.Count(s => s == ScientificSymbol.Gear));
        }

        [Fact]
        public void GetBestCombinationTwoAndTwoSymbolsTest()
        {
            var input = new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Gear, ScientificSymbol.Tablet, ScientificSymbol.Tablet };
            var output1 = ScientificStructuresCategory.GetBestCombination(input, 1);

            Assert.Equal(1, output1.Count(s => s == ScientificSymbol.Compass));
        }

        [Fact]
        public void GetBestCombinationThreeAndOneAndOneSymbolsTest()
        {
            var input = new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Gear, ScientificSymbol.Gear, ScientificSymbol.Tablet, ScientificSymbol.Compass };
            var output1 = ScientificStructuresCategory.GetBestCombination(input, 1);

            var output2 = ScientificStructuresCategory.GetBestCombination(input, 2);

            Assert.Equal(4, output1.Count(s => s == ScientificSymbol.Gear));

            Assert.Equal(5, output2.Count(s => s == ScientificSymbol.Gear));
        }

        [Theory, AutoGameSetupData]
        public void ComputeScientificCardsTest(ScientificStructuresCategory pointsCategory, List<GamePlayer> players)
        {
            var cards = CreateCards();

            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];

            player1.SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.A));
            player2.SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.A));
            player3.SetWonder(WonderFactory.CreateWonder(WonderName.LighthouseOfAlexandria, WonderBoardSide.A));

            player2.Wonder.BuildStage();
            player2.Wonder.BuildStage();

            //2 tablets, 2 compass, 1 random
            player1.Cards.Add(cards.First(c => c.Name == CardName.Scriptorium));
            player1.Cards.Add(cards.First(c => c.Name == CardName.Dispensary));
            player1.Cards.Add(cards.First(c => c.Name == CardName.Library));
            player1.Cards.Add(cards.First(c => c.Name == CardName.Lodge));
            player1.Cards.Add(cards.First(c => c.Name == CardName.ScientistsGuild));

            //3 compass, 1 gear, 2 random
            player2.Cards.Add(cards.First(c => c.Name == CardName.ScientistsGuild));
            player2.Cards.Add(cards.First(c => c.Name == CardName.Apothecary));
            player2.Cards.Add(cards.First(c => c.Name == CardName.Lodge));
            player2.Cards.Add(cards.First(c => c.Name == CardName.Dispensary));
            player2.Cards.Add(cards.First(c => c.Name == CardName.Observatory));

            //1 each
            player3.Cards.Add(cards.First(c => c.Name == CardName.University));
            player3.Cards.Add(cards.First(c => c.Name == CardName.Lodge));
            player3.Cards.Add(cards.First(c => c.Name == CardName.Observatory));

            pointsCategory.ComputePoints(players);

            Assert.Equal(16, player1.VictoryPoints);
            Assert.Equal(26, player2.VictoryPoints);
            Assert.Equal(10, player3.VictoryPoints);
        }

        private List<StructureCard> CreateCards()
        {
            return new List<StructureCard>
            {
                new ScientificCard(CardName.Apothecary, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Compass) }),
                new ScientificCard(CardName.Workshop, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Gear) }),
                new ScientificCard(CardName.Scriptorium, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Tablet) }),
                new ScientificCard(CardName.Dispensary, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Compass) }),
                new ScientificCard(CardName.Laboratory, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Gear) }),
                new ScientificCard(CardName.Library, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Tablet) }),
                new ScientificCard(CardName.Lodge, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Compass) }),
                new ScientificCard(CardName.Observatory, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Gear) }),
                new ScientificCard(CardName.University, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Tablet) }),
                new GuildCard(CardName.ScientistsGuild, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.Compass), new Effect(EffectType.Gear), new Effect(EffectType.Tablet) }),
            };
        }

    }
}
