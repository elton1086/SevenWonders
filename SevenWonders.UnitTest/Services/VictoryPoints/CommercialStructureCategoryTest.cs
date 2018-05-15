using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Entities;
using SevenWonder.Factories;
using SevenWonder.Services.VictoryPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.UnitTest.Services.VictoryPoints
{
    [TestClass]
    public class CommercialStructureCategoryTest
    {
        private List<IPlayer> players;
        IEnumerable<IStructureCard> cards;

        [TestInitialize]
        public void Initialize()
        {
            CreateCards();

            players = new List<IPlayer>
            {
                new TurnPlayer("jennifer"),
                new TurnPlayer("jessica"),
                new TurnPlayer("amanda")
            };
            ((IGamePlayer)players[0]).SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.A));
            ((IGamePlayer)players[1]).SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.A));
            ((IGamePlayer)players[2]).SetWonder(WonderFactory.CreateWonder(WonderName.LighthouseOfAlexandria, WonderBoardSide.A));
        }

        public void CreateCards()
        {
            cards = new List<IStructureCard>
            {
                new CommercialCard(CardName.ChamberOfCommerce, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.CoinPerManufacturedGoodCard, 2, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerManufacturedGoodCard, 2, PlayerDirection.Myself) }),
                new CommercialCard(CardName.Haven, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.CoinPerRawMaterialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerRawMaterialCard, 1, PlayerDirection.Myself) }),
                new CommercialCard(CardName.Lighthouse, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.CoinPerCommercialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerCommercialCard, 1, PlayerDirection.Myself) }),
                new CommercialCard(CardName.Arena, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.CoinPerWonderStageBuilt, 3, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerWonderStageBuilt, 1, PlayerDirection.Myself) }),
                new CommercialCard(CardName.Forum, 3, Age.II, null, null, new List<IEffect> { new Effect(EffectType.Loom), new Effect(EffectType.Glass), new Effect(EffectType.Papyrus) }),
                new RawMaterialCard(CardName.LumberYard, 4, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Wood) }),
                new RawMaterialCard(CardName.TimberYard, 3, Age.I, new List<ResourceType>{ ResourceType.Coin }, null, new List<IEffect> { new Effect(EffectType.Stone), new Effect(EffectType.Wood) }),
                new ManufacturedGoodCard(CardName.Glassworks, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Glass) }),
                new ManufacturedGoodCard(CardName.Press, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Papyrus) }),
            };
        }

        [TestMethod]
        public void ComputePointsByWonderStageBuiltTest()
        {
            players[0].Wonder.BuildStage();
            players[1].Wonder.BuildStage();
            players[1].Wonder.BuildStage();

            players[1].Cards.Add(cards.First(c => c.Name == CardName.Arena));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Arena));

            var pointsCategory = new CommercialStructuresCategory();
            pointsCategory.ComputePoints(players);

            Assert.AreEqual(0, players[0].VictoryPoints);
            Assert.AreEqual(2, players[1].VictoryPoints);
            Assert.AreEqual(0, players[2].VictoryPoints);
        }

        [TestMethod]
        public void ComputePointsByCommercialTest()
        {
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Lighthouse));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Forum));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Haven));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Forum));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.ChamberOfCommerce));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Lighthouse));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Forum));

            var pointsCategory = new CommercialStructuresCategory();
            pointsCategory.ComputePoints(players);

            Assert.AreEqual(2, players[0].VictoryPoints);
            Assert.AreEqual(0, players[1].VictoryPoints);
            Assert.AreEqual(2, players[2].VictoryPoints);
        }

        [TestMethod]
        public void ComputePointsByRawMaterialTest()
        {
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Haven));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.TimberYard));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Haven));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Glassworks));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Forum));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.TimberYard));

            var pointsCategory = new CommercialStructuresCategory();
            pointsCategory.ComputePoints(players);

            Assert.AreEqual(2, players[0].VictoryPoints);
            Assert.AreEqual(0, players[1].VictoryPoints);
            Assert.AreEqual(0, players[2].VictoryPoints);
        }

        [TestMethod]
        public void ComputePointsByManufacturedGoodTest()
        {
            players[0].Cards.Add(cards.First(c => c.Name == CardName.ChamberOfCommerce));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Glassworks));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Press));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.ChamberOfCommerce));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Glassworks));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.TimberYard));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Press));

            var pointsCategory = new CommercialStructuresCategory();
            pointsCategory.ComputePoints(players);

            Assert.AreEqual(4, players[0].VictoryPoints);
            Assert.AreEqual(2, players[1].VictoryPoints);
            Assert.AreEqual(0, players[2].VictoryPoints);
        }

        [TestMethod]
        public void ComputePointsByManyTypesTest()
        {
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

            var pointsCategory = new CommercialStructuresCategory();
            pointsCategory.ComputePoints(players);

            //4 for manufactured cards, 2 for commercial cards
            Assert.AreEqual(6, players[0].VictoryPoints);
            //2 for manufacured cards, 2 for wonder stages, 1 for raw material cards
            Assert.AreEqual(5, players[1].VictoryPoints);
            //3 for commercial cards, 1 for wonder stages
            Assert.AreEqual(4, players[2].VictoryPoints);
        }
    }
}
