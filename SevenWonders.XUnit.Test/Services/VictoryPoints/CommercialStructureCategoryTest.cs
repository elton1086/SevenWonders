using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Factories;
using SevenWonders.Services.VictoryPoints;
using SevenWonders.XUnit.Test.AutoData;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SevenWonders.UnitTest.Services.VictoryPoints
{
    public class CommercialStructureCategoryTest
    {
        private List<StructureCard> CreateCards()
        {
            return new List<StructureCard>
            {
                new CommercialCard(CardName.ChamberOfCommerce, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.CoinPerManufacturedGoodCard, 2, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerManufacturedGoodCard, 2, PlayerDirection.Myself) }),
                new CommercialCard(CardName.Haven, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.CoinPerRawMaterialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerRawMaterialCard, 1, PlayerDirection.Myself) }),
                new CommercialCard(CardName.Lighthouse, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.CoinPerCommercialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerCommercialCard, 1, PlayerDirection.Myself) }),
                new CommercialCard(CardName.Arena, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.CoinPerWonderStageBuilt, 3, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerWonderStageBuilt, 1, PlayerDirection.Myself) }),
                new CommercialCard(CardName.Forum, 3, Age.II, null, null, new List<Effect> { new Effect(EffectType.Loom), new Effect(EffectType.Glass), new Effect(EffectType.Papyrus) }),
                new RawMaterialCard(CardName.LumberYard, 4, Age.I, null, null, new List<Effect> { new Effect(EffectType.Wood) }),
                new RawMaterialCard(CardName.TimberYard, 3, Age.I, new List<ResourceType>{ ResourceType.Coin }, null, new List<Effect> { new Effect(EffectType.Stone), new Effect(EffectType.Wood) }),
                new ManufacturedGoodCard(CardName.Glassworks, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.Glass) }),
                new ManufacturedGoodCard(CardName.Press, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.Papyrus) }),
            };
        }

        [Theory, AutoBaseGameSetupData]
        public void ComputePointsByWonderStageBuiltTest(CommercialStructuresCategory pointsCategory, List<GamePlayer> players)
        {
            players[0].SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.A));
            players[1].SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.A));
            players[2].SetWonder(WonderFactory.CreateWonder(WonderName.LighthouseOfAlexandria, WonderBoardSide.A));

            players[0].Wonder.BuildStage();
            players[1].Wonder.BuildStage();
            players[1].Wonder.BuildStage();

            var card = new CommercialCard(CardName.Arena, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.CoinPerWonderStageBuilt, 3, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerWonderStageBuilt, 1, PlayerDirection.Myself) });
            players[1].Cards.Add(card);
            players[2].Cards.Add(card);

            pointsCategory.ComputePoints(players);

            Assert.Equal(0, players[0].VictoryPoints);
            Assert.Equal(2, players[1].VictoryPoints);
            Assert.Equal(0, players[2].VictoryPoints);
        }

        [Theory, AutoBaseGameSetupData]
        public void ComputePointsByCommercialTest(CommercialStructuresCategory pointsCategory, List<GamePlayer> players)
        {
            var cards = CreateCards();
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Lighthouse));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Forum));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Haven));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Forum));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.ChamberOfCommerce));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Lighthouse));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Forum));

            pointsCategory.ComputePoints(players);

            Assert.Equal(2, players[0].VictoryPoints);
            Assert.Equal(0, players[1].VictoryPoints);
            Assert.Equal(2, players[2].VictoryPoints);
        }

        [Theory, AutoBaseGameSetupData]
        public void ComputePointsByRawMaterialTest(CommercialStructuresCategory pointsCategory, List<GamePlayer> players)
        {
            var cards = CreateCards();
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Haven));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.TimberYard));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Haven));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Glassworks));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Forum));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.TimberYard));

            pointsCategory.ComputePoints(players);

            Assert.Equal(2, players[0].VictoryPoints);
            Assert.Equal(0, players[1].VictoryPoints);
            Assert.Equal(0, players[2].VictoryPoints);
        }

        [Theory, AutoBaseGameSetupData]
        public void ComputePointsByManufacturedGoodTest(CommercialStructuresCategory pointsCategory, List<GamePlayer> players)
        {
            var cards = CreateCards();
            players[0].Cards.Add(cards.First(c => c.Name == CardName.ChamberOfCommerce));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Glassworks));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Press));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.ChamberOfCommerce));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Glassworks));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.TimberYard));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Press));

            pointsCategory.ComputePoints(players);

            Assert.Equal(4, players[0].VictoryPoints);
            Assert.Equal(2, players[1].VictoryPoints);
            Assert.Equal(0, players[2].VictoryPoints);
        }

        [Theory, AutoBaseGameSetupData]
        public void ComputePointsByManyTypesTest(CommercialStructuresCategory pointsCategory, List<GamePlayer> players)
        {
            var cards = CreateCards();
            players[0].SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.A));
            players[1].SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.A));
            players[2].SetWonder(WonderFactory.CreateWonder(WonderName.LighthouseOfAlexandria, WonderBoardSide.A));

            players[0].Wonder.BuildStage();
            players[0].Cards.Add(cards.First(c => c.Name == CardName.ChamberOfCommerce));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Glassworks));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Press));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.TimberYard));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Lighthouse));

            players[1].Wonder.BuildStage();
            players[1].Wonder.BuildStage();
            players[1].Cards.Add(cards.First(c => c.Name == CardName.ChamberOfCommerce));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Glassworks));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.TimberYard));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Arena));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Haven));

            players[2].Wonder.BuildStage();
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Lighthouse));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Arena));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Forum));

            pointsCategory.ComputePoints(players);

            //4 for manufactured cards, 2 for commercial cards
            Assert.Equal(6, players[0].VictoryPoints);
            //2 for manufacured cards, 2 for wonder stages, 1 for raw material cards
            Assert.Equal(5, players[1].VictoryPoints);
            //3 for commercial cards, 1 for wonder stages
            Assert.Equal(4, players[2].VictoryPoints);
        }
    }
}
