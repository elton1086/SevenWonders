using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Entities;
using SevenWonder.Factories;
using SevenWonder.Services.VictoryPoints;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.UnitTest.Services.VictoryPoints
{
    [TestClass]
    public class GuildsCategoryTest
    {
        List<IPlayer> players;
        List<IStructureCard> cards;

        [TestInitialize]
        public void Initialize()
        {
            CreateCards();

            players = new List<IPlayer>
            {
                new TurnPlayer("angel"),
                new TurnPlayer("barbara"),
                new TurnPlayer("amy")
            };
            ((IGamePlayer)players[0]).SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.B));
            ((IGamePlayer)players[1]).SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.A));
            ((IGamePlayer)players[2]).SetWonder(WonderFactory.CreateWonder(WonderName.LighthouseOfAlexandria, WonderBoardSide.A));
        }

        public void CreateCards()
        {
            cards = new List<IStructureCard>
            {
                new GuildCard(CardName.SpiesGuild, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.VictoryPointPerMilitaryCard, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) }),
                new GuildCard(CardName.CraftsmenGuild, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.VictoryPointPerManufacturedGoodCard, 2, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) }),
                new GuildCard(CardName.StrategistsGuild, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.VictoryPointPerConflictDefeat, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) }),
                new GuildCard(CardName.ShipownersGuild, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.VictoryPointPerRawMaterialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerManufacturedGoodCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerGuildCard, 1, PlayerDirection.Myself) }),
                new GuildCard(CardName.BuildersGuild, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.VictoryPointPerWonderStageBuilt, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight | PlayerDirection.Myself) }),
                new MilitaryCard(CardName.SiegeWorkshop, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.Shield, 3) }),
                new MilitaryCard(CardName.Fortification, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.Shield, 3) }),
                new RawMaterialCard(CardName.LumberYard, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Wood) }),
                new RawMaterialCard(CardName.Brickyard, 3, Age.II, null, null, new List<IEffect> { new Effect(EffectType.Clay, 2) }),
                new ManufacturedGoodCard(CardName.Loom, 3, Age.II, null, null, new List<IEffect> { new Effect(EffectType.Loom) }),
            };
        }

        [TestMethod]
        public void ComputeTest()
        {
            var player1 = (ITurnPlayer)players[0];
            var player2 = (ITurnPlayer)players[1];
            var player3 = (ITurnPlayer)players[2];

            player1.Wonder.BuildStage();
            player1.Wonder.BuildStage();
            player2.Wonder.BuildStage();
            player3.Wonder.BuildStage();
            player3.Wonder.BuildStage();

            player1.ConflictTokens.Add(ConflictToken.Defeat);
            player1.ConflictTokens.Add(ConflictToken.Defeat);
            player1.ConflictTokens.Add(ConflictToken.AgeThreeVictory);
            player3.ConflictTokens.Add(ConflictToken.Defeat);
            player3.ConflictTokens.Add(ConflictToken.Defeat);
            player3.ConflictTokens.Add(ConflictToken.Defeat);
            player3.ConflictTokens.Add(ConflictToken.AgeOneVictory);

            player1.Cards.Add(cards.First(c => c.Name == CardName.SpiesGuild));
            player1.Cards.Add(cards.First(c => c.Name == CardName.CraftsmenGuild));

            player2.Cards.Add(cards.First(c => c.Name == CardName.StrategistsGuild));
            player2.Cards.Add(cards.First(c => c.Name == CardName.BuildersGuild));
            player2.Cards.Add(cards.First(c => c.Name == CardName.SiegeWorkshop));
            player2.Cards.Add(cards.First(c => c.Name == CardName.Fortification));

            player3.Cards.Add(cards.First(c => c.Name == CardName.ShipownersGuild));
            player3.Cards.Add(cards.First(c => c.Name == CardName.Fortification));
            player3.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player3.Cards.Add(cards.First(c => c.Name == CardName.Brickyard));
            player3.Cards.Add(cards.First(c => c.Name == CardName.Loom));

            var pointsCategory = new GuildsCategory();
            pointsCategory.ComputePoints(players);

            Assert.AreEqual(5, player1.VictoryPoints);
            Assert.AreEqual(10, player2.VictoryPoints);
            Assert.AreEqual(4, player3.VictoryPoints);
        }
    }
}
