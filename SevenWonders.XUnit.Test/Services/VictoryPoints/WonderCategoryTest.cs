using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Factories;
using SevenWonders.Services.VictoryPoints;
using SevenWonders.XUnit.Test.AutoData;
using System.Collections.Generic;
using Xunit;

namespace SevenWonders.UnitTest.Services.VictoryPoints
{
    public class WonderCategoryTest
    {
        [Theory, AutoGameSetupData]
        public void ComputeTest(WonderCategory pointsCategory, List<GamePlayer> players)
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

            pointsCategory.ComputePoints(players);

            Assert.Equal(5, players[0].VictoryPoints);
            Assert.Equal(3, players[1].VictoryPoints);
            Assert.Equal(3, players[2].VictoryPoints);
        }
    }
}
