using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenWonder.Services;
using SevenWonder.Contracts;
using SevenWonder.Entities;
using SevenWonder.Services.Contracts;
using SevenWonder.Utilities;
using SevenWonder.BaseEntities;
using SevenWonder.Factories;

namespace SevenWonders.UnitTest.Services
{
    [TestClass]
    public class TradeManagerTest
    {
        ITradeManager manager;
        List<IGamePlayer> players;
        IEnumerable<IStructureCard> cards;

        [TestInitialize]
        public void Initialize()
        {
            manager = new TradeManager(new NullUnitOfWork());

            players = new List<IGamePlayer>
            {
                new TurnPlayer("angel"),
                new TurnPlayer("joseph"),
                new TurnPlayer("tina"),
                new TurnPlayer("laura"),
                new TurnPlayer("larry"),
            };

            players[0].SetWonder(WonderFactory.CreateWonder(WonderName.ColossusOfRhodes, WonderBoardSide.B));
            players[1].SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.A));
            players[2].SetWonder(WonderFactory.CreateWonder(WonderName.LighthouseOfAlexandria, WonderBoardSide.A));
            players[3].SetWonder(WonderFactory.CreateWonder(WonderName.MausoleumOfHalicarnassus, WonderBoardSide.A));
            players[4].SetWonder(WonderFactory.CreateWonder(WonderName.PyramidsOfGiza, WonderBoardSide.A));

            CreateCards();            
        }

        public void CreateCards()
        {
            cards = new List<IStructureCard>
            {
                new MilitaryCard(CardName.Stockade, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Shield, 1) }),
                new MilitaryCard(CardName.Stables, 3, Age.II, null, null, new List<IEffect> { new Effect(EffectType.Shield, 2) }),                
                new MilitaryCard(CardName.Circus, 3, Age.III, null, null, new List<IEffect> { new Effect(EffectType.Shield, 3) }),
            };
        }

        [TestMethod]
        public void SetupCoinsFromBankTest()
        {
            var players = new List<IGamePlayer>();
            players.Add(new TurnPlayer("travis"));
            players.Add(new TurnPlayer("joe"));
            manager.SetupCoinsFromBank(players);
            Assert.AreEqual(players[0].Coins, 3);
        }

        [TestMethod]
        public void SetupCoinsFromBankNoPlayerTest()
        {
            var players = new List<IGamePlayer>();
            manager.SetupCoinsFromBank(players);
            Assert.AreEqual(0, players.Count);
        }

        [TestMethod]
        public void ResolveMilitaryConflictsForAgeOneTest()
        {            
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            players[3].Cards.Add(cards.First(c => c.Name == CardName.Stockade));
            manager.ResolveMilitaryConflicts(players, Age.I);

            Assert.AreEqual(0, players[0].ConflictTokenSum);
            Assert.AreEqual(2, players[1].ConflictTokenSum);
            Assert.AreEqual(-1, players[2].ConflictTokenSum);
            Assert.AreEqual(1, players[3].ConflictTokenSum);
            Assert.AreEqual(-2, players[4].ConflictTokenSum);
        }

        [TestMethod]
        public void ResolveMilitaryConflictsForAgeTwoTest()
        {
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

            Assert.AreEqual(6, players[0].ConflictTokenSum);
            Assert.AreEqual(2, players[1].ConflictTokenSum);
            Assert.AreEqual(-1, players[2].ConflictTokenSum);
            Assert.AreEqual(-1, players[3].ConflictTokenSum);
            Assert.AreEqual(2, players[4].ConflictTokenSum);
        }

        [TestMethod]
        public void ResolveMilitaryConflictsForAgeThreeTest()
        {
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

            players[0].Wonder.BuildStage();
            players[0].Wonder.BuildStage();

            manager.ResolveMilitaryConflicts(players, Age.III);

            Assert.AreEqual(5, players[0].ConflictTokenSum);
            Assert.AreEqual(-1, players[1].ConflictTokenSum);
            Assert.AreEqual(10, players[2].ConflictTokenSum);
            Assert.AreEqual(4, players[3].ConflictTokenSum);
            Assert.AreEqual(-2, players[4].ConflictTokenSum);
        }
    }
}
