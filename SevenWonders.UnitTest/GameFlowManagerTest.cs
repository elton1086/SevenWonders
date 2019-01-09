using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Entities;
using SevenWonder.Factories;
using SevenWonder.Services;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.UnitTest
{
    [TestClass]
    public class GameFlowManagerTest
    {
        [TestMethod]
        public void SetupGameFor5PlayersTest()
        {
            var manager = new GameFlowManager();
            manager.CreateNewPlayer("maria");
            manager.CreateNewPlayer("joão");
            manager.CreateNewPlayer("josé");
            manager.CreateNewPlayer("ana");
            manager.CreateNewPlayer("pedro");
            manager.SetupGame();
            Assert.AreEqual(5, manager.Players.Count);
            //Number of players X 3 ages X 7 cards per age
            Assert.AreEqual(105, manager.FullDeckOfCards.Count);
            //Base game has 7 wonders - number of players
            Assert.AreEqual(2, manager.WonderCards.Count);
            Assert.AreNotEqual(manager.Players[1].Wonder.Name, manager.Players[3].Wonder.Name);
        }

        [TestMethod]
        public void DealAgeCardsFor3PlayersTest()
        {
            var manager = new GameFlowManager();
            manager.CreateNewPlayer("paul");
            manager.CreateNewPlayer("mary");
            manager.CreateNewPlayer("tracy");
            manager.SetupGame();
            manager.StartAge();
            Assert.IsFalse(manager.Players.Any(p => p.SelectableCards.Count != 7));
            Assert.IsFalse(manager.Players.Any(p => p.SelectableCards.Any(c => c.Age != Age.I)));
        }

        [TestMethod]
        public void PlayTurnTest()
        {
            var manager = new GameFlowManager();
            manager.CreateNewPlayer("paul");
            manager.CreateNewPlayer("mary");
            manager.CreateNewPlayer("tracy");
            manager.SetupGame();
            manager.StartAge();
            for (int i = 0; i < 7; i++)
            {
                foreach(var p in manager.Players)
                {
                    p.SelectedCard = p.SelectableCards[0];
                    p.ChosenAction = TurnAction.BuyCard;
                }
                manager.PlayTurn();
                manager.CollectTurnRewards();
                manager.EndTurn();
            }
        }

        [TestMethod]
        public void ComputeCopyBuildCardTest()
        {
            var manager = new GameFlowManager();
            var cardName = CardName.SpiesGuild;
            var guild = new GuildCard(cardName, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.VictoryPointPerMilitaryCard, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) });
            manager.CreateNewPlayer("ashley");
            manager.CreateNewPlayer("kate");
            manager.SetupGame();
            var player1 = manager.Players[0];
            var player2 = manager.Players[1];

            player1.SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.B));
            player1.Wonder.BuildStage();
            player1.Wonder.BuildStage();
            player1.Wonder.BuildStage();
            player1.Wonder.EffectsAvailable.First(e => e.Type == EffectType.CopyGuildFromNeighbor).Info = cardName;

            player2.Cards.Add(guild);

            manager.CollectPostGameRewards();

            Assert.IsTrue(player1.Cards.Any(c => c.Name == cardName));
        }
    }
}
