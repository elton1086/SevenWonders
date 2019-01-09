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
        List<Player> players;

        public WonderCategoryTest()
        {
            players = new List<Player>
            {
                new TurnPlayer("angel"),
                new TurnPlayer("barbara"),
                new TurnPlayer("amy")
            };
            ((GamePlayer)players[0]).SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.B));
            ((GamePlayer)players[1]).SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.A));
            ((GamePlayer)players[2]).SetWonder(WonderFactory.CreateWonder(WonderName.LighthouseOfAlexandria, WonderBoardSide.A));
        }

        [Theory, AutoMoqData]
        public void ComputeTest(WonderCategory pointsCategory)
        {
            var player1 = (TurnPlayer)players[0];
            var player2 = (TurnPlayer)players[1];
            var player3 = (TurnPlayer)players[2];

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
