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
    public class WonderCategoryTest
    {
        List<IPlayer> players;

        [TestInitialize]
        public void Initialize()
        {
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

            var pointsCategory = new WonderCategory();
            pointsCategory.ComputePoints(players);

            Assert.AreEqual(5, players[0].VictoryPoints);
            Assert.AreEqual(3, players[1].VictoryPoints);
            Assert.AreEqual(3, players[2].VictoryPoints);
        }
    }
}
