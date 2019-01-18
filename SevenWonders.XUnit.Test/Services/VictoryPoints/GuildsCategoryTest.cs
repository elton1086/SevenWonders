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
    public class GuildsCategoryTest
    {
        private List<StructureCard> CreateCards()
        {
            return new List<StructureCard>
            {
                new GuildCard(CardName.SpiesGuild, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.VictoryPointPerMilitaryCard, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) }),
                new GuildCard(CardName.CraftsmenGuild, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.VictoryPointPerManufacturedGoodCard, 2, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) }),
                new GuildCard(CardName.StrategistsGuild, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.VictoryPointPerConflictDefeat, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) }),
                new GuildCard(CardName.ShipownersGuild, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.VictoryPointPerRawMaterialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerManufacturedGoodCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerGuildCard, 1, PlayerDirection.Myself) }),
                new GuildCard(CardName.BuildersGuild, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.VictoryPointPerWonderStageBuilt, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight | PlayerDirection.Myself) }),
                new MilitaryCard(CardName.SiegeWorkshop, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Shield, 3) }),
                new MilitaryCard(CardName.Fortification, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.Shield, 3) }),
                new RawMaterialCard(CardName.LumberYard, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.Wood) }),
                new RawMaterialCard(CardName.Brickyard, 3, Age.II, null, null, new List<Effect> { new Effect(EffectType.Clay, 2) }),
                new ManufacturedGoodCard(CardName.Loom, 3, Age.II, null, null, new List<Effect> { new Effect(EffectType.Loom) }),
            };
        }

        [Theory, AutoGameSetupData]
        public void ComputeTest(GuildsCategory pointsCategory, List<GamePlayer> players)
        {
            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];

            player1.SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.B));
            player2.SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.A));
            player3.SetWonder(WonderFactory.CreateWonder(WonderName.LighthouseOfAlexandria, WonderBoardSide.A));

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

            var cards = CreateCards();
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

            pointsCategory.ComputePoints(players);

            Assert.Equal(5, player1.VictoryPoints);
            Assert.Equal(10, player2.VictoryPoints);
            Assert.Equal(4, player3.VictoryPoints);
        }
    }
}
