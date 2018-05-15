using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenWonder.Contracts;
using SevenWonder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SevenWonder.BaseEntities;
using SevenWonder.Services;
using SevenWonder.Utilities;
using SevenWonder.CardGenerator;
using SevenWonder.Factories;
using SevenWonder.Services.Contracts;

namespace SevenWonders.UnitTest.Services
{
    [TestClass]
    public class TurnManagerTest
    {
        private List<ITurnPlayer> players;
        IEnumerable<IStructureCard> cards;

        [TestInitialize]
        public void Initialize()
        {
            players = new List<ITurnPlayer>
            {
                new TurnPlayer("jennifer"),
                new TurnPlayer("jessica"),
                new TurnPlayer("amanda")
            };

            CreateCards();

            IStructureCard caravansery = cards.First(c => c.Name == CardName.Caravansery);
            IStructureCard foundry = cards.First(c => c.Name == CardName.Foundry);
            IStructureCard laboratory = cards.First(c => c.Name == CardName.Laboratory);
            IStructureCard oreVein = cards.First(c => c.Name == CardName.OreVein);

            #region Player 1
            var player1 = players[0];
            player1.SetSelectableCards(new List<IStructureCard>
            {
                caravansery,
                foundry,
                laboratory,
                oreVein
            });
            player1.SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.A));
            player1.ReceiveCoin(ConstantValues.INITIAL_COINS);
            player1.InitializeTurnData();
            #endregion

            #region Player 2
            players[1].SetSelectableCards(new List<IStructureCard>
            {
                foundry,
                caravansery,
                laboratory,
                oreVein
            });
            players[1].SetWonder(WonderFactory.CreateWonder(WonderName.LighthouseOfAlexandria, WonderBoardSide.A));
            players[1].ReceiveCoin(ConstantValues.INITIAL_COINS);
            players[1].InitializeTurnData();
            #endregion

            #region Player 3
            players[2].SetSelectableCards(new List<IStructureCard>
            {
                laboratory,
                caravansery,
                foundry,
                oreVein
            });
            players[2].SetWonder(WonderFactory.CreateWonder(WonderName.TempleOfArthemisInEphesus, WonderBoardSide.A));
            players[2].ReceiveCoin(ConstantValues.INITIAL_COINS);
            players[2].InitializeTurnData();
            #endregion
        }

        public void CreateCards()
        {
            cards = new List<IStructureCard>
            {
                new CommercialCard(CardName.Caravansery, 3, Age.II, new List<ResourceType>{ ResourceType.Wood, ResourceType.Wood }, new List<CardName> { CardName.Marketplace }, new List<IEffect> { new Effect(EffectType.Clay), new Effect(EffectType.Wood), new Effect(EffectType.Stone), new Effect(EffectType.Ore) }),
                new RawMaterialCard(CardName.LumberYard, 4, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Wood) }),
                new RawMaterialCard(CardName.TimberYard, 3, Age.I, new List<ResourceType>{ ResourceType.Coin }, null, new List<IEffect> { new Effect(EffectType.Stone), new Effect(EffectType.Wood) }),
                new RawMaterialCard(CardName.Foundry, 3, Age.II, new List<ResourceType>{ ResourceType.Coin }, null, new List<IEffect> { new Effect(EffectType.Ore), new Effect(EffectType.Ore) }),
                new ScientificCard(CardName.Workshop, 3, Age.I, new List<ResourceType>{ ResourceType.Glass }, null, new List<IEffect> { new Effect(EffectType.Gear) }),
                new ScientificCard(CardName.Laboratory, 3, Age.II, new List<ResourceType>{ ResourceType.Clay, ResourceType.Clay, ResourceType.Papyrus }, new List<CardName> { CardName.Workshop}, new List<IEffect> { new Effect(EffectType.Gear) }),
                new RawMaterialCard(CardName.TreeFarm, 6, Age.I, new List<ResourceType>{ ResourceType.Coin }, null, new List<IEffect> { new Effect(EffectType.Clay), new Effect(EffectType.Wood) }),
                new CivilianCard(CardName.Temple, 3, Age.II, new List<ResourceType>{ ResourceType.Clay, ResourceType.Wood, ResourceType.Glass }, new List<CardName> { CardName.Altar}, new List<IEffect> { new Effect(EffectType.VictoryPoint, 3) }),
                new ManufacturedGoodCard(CardName.Glassworks, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Glass) }),
                new RawMaterialCard(CardName.ClayPool, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Clay) }),
                new RawMaterialCard(CardName.Brickyard, 3, Age.II, new List<ResourceType>{ ResourceType.Coin }, null, new List<IEffect> { new Effect(EffectType.Clay, 2) }),
                new RawMaterialCard(CardName.OreVein, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Ore) }),
                new CommercialCard(CardName.WestTradingPost, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.BuyRawMaterialDiscount, 1, PlayerDirection.ToTheLeft) }),
                new CivilianCard(CardName.Baths, 3, Age.I, new List<ResourceType>{ ResourceType.Stone }, null, new List<IEffect> { new Effect(EffectType.VictoryPoint, 3) }),
                new RawMaterialCard(CardName.StonePit, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Stone) }),
                new MilitaryCard(CardName.Barracks, 3, Age.I, new List<ResourceType>{ ResourceType.Ore }, null, new List<IEffect> { new Effect(EffectType.Shield) }),
                new ScientificCard(CardName.Apothecary, 3, Age.I, new List<ResourceType>{ ResourceType.Loom }, null, new List<IEffect> { new Effect(EffectType.Compass) }),
                new ManufacturedGoodCard(CardName.Loom, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.Loom) }),
                new CommercialCard(CardName.Vineyard, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.CoinPerRawMaterialCard, 1, PlayerDirection.Myself | PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) }),
                new CommercialCard(CardName.Arena, 3, Age.I, null, null, new List<IEffect> { new Effect(EffectType.CoinPerWonderStageBuilt, 3, PlayerDirection.Myself) }),
            };
        }

        [TestMethod]
        public void MoveSelectableCardsToLeftTest()
        {
            var manager = new TurnManager();
            manager.SetCurrentInfo(Age.I, 1);
            var cardOne = players[0].SelectableCards[0].Name;
            var cardTwo = players[1].SelectableCards[0].Name;
            var cardThree = players[2].SelectableCards[0].Name;
            manager.MoveSelectableCards(players);
            Assert.AreEqual(cardOne, players[1].SelectableCards[0].Name);
            Assert.AreEqual(cardTwo, players[2].SelectableCards[0].Name);
            Assert.AreEqual(cardThree, players[0].SelectableCards[0].Name);
        }

        [TestMethod]
        public void MoveSelectableCardsToRightTest()
        {
            var manager = new TurnManager();
            manager.SetCurrentInfo(Age.II, 1);
            var cardOne = players[0].SelectableCards[0].Name;
            var cardTwo = players[1].SelectableCards[0].Name;
            var cardThree = players[2].SelectableCards[0].Name;
            manager.MoveSelectableCards(players);
            Assert.AreEqual(cardOne, players[2].SelectableCards[0].Name);
            Assert.AreEqual(cardTwo, players[0].SelectableCards[0].Name);
            Assert.AreEqual(cardThree, players[1].SelectableCards[0].Name);
        }

        [TestMethod]
        public void SellCardTest()
        {
            var player = players[0];
            player.ChosenAction = TurnAction.SellCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            var discarded = new List<IStructureCard>();
            manager.Play(new List<ITurnPlayer> { player }, discarded);
            uow.Commit();
            Assert.IsTrue(discarded.Any(c => c.Name == CardName.Caravansery));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, player.Coins);
            Assert.IsFalse(player.Cards.Any(c => c.Name == CardName.Caravansery));
        }

        [TestMethod]
        public void BuyCardPayCoin()
        {
            var player = players[0];
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectableCards[0] = cards.First(c => c.Name == CardName.TimberYard);
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.TimberYard);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(new List<ITurnPlayer> { player }, new List<IStructureCard>());
            uow.Commit();
            Assert.IsTrue(player.Cards.Any(c => c.Name == CardName.TimberYard));
            Assert.AreEqual(ConstantValues.INITIAL_COINS - 1, player.Coins);
        }

        [TestMethod]
        public void BuyCardWithOwnResourcesTest()
        {
            var player = players[0];
            player.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(new List<ITurnPlayer> { player }, new List<IStructureCard>());
            uow.Commit();
            Assert.IsTrue(player.Cards.Any(c => c.Name == CardName.Caravansery));
            Assert.AreEqual(ConstantValues.INITIAL_COINS, player.Coins);
        }

        [TestMethod]
        public void BuyCardForFreeTest()
        {
            players[0].Cards.Add(cards.First(c => c.Name == CardName.Workshop));
            players[0].ChosenAction = TurnAction.BuyCard;
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(new List<ITurnPlayer> { players[0] }, new List<IStructureCard>());
            uow.Commit();
            Assert.IsTrue(players[0].Cards.Any(c => c.Name == CardName.Laboratory));
        }

        [TestMethod]
        public void BuyCardUsingSpecialCaseTest()
        {
            players[0].ChosenAction = TurnAction.BuyCard;
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);
            players[0].Wonder.BuildStage();
            players[0].Wonder.BuildStage();
            players[0].SpecialCaseToUse = SpecialCaseType.PlayCardForFreeOncePerAge;

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(new List<ITurnPlayer> { players[0] }, new List<IStructureCard>());
            uow.Commit();
            Assert.IsTrue(players[0].Cards.Any(c => c.Name == CardName.Laboratory));
            Assert.IsTrue((Age)players[0].GetNonResourceEffects().First(e => e.Type == EffectType.PlayCardForFreeOncePerAge).Info == Age.II);
        }

        [TestMethod]
        public void BuyCardUsingChoosableResouceTest()
        {
            //Produces 1 wood and 1 clay / wood
            //Costs 2 woods
            players[0].Cards.Add(cards.First(c => c.Name == CardName.TreeFarm));
            players[0].ChosenAction = TurnAction.BuyCard;
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(new List<ITurnPlayer> { players[0] }, new List<IStructureCard>());
            uow.Commit();
            Assert.IsTrue(players[0].Cards.Any(c => c.Name == CardName.Caravansery));
            Assert.AreEqual(ConstantValues.INITIAL_COINS, players[0].Coins);
        }

        [TestMethod]
        public void BuyCardUsingChoosableResouceFromWonderStageTest()
        {
            var player = players[1];
            //Produces 1 glass and 1 clay / wood and 1 of any raw material (2nd stage)      
            player.Cards.Add(cards.First(c => c.Name == CardName.TreeFarm));
            player.ChosenAction = TurnAction.BuyCard;
            //Costs 1 wood, 1 clay and 1 glass
            player.SelectableCards[0] = cards.First(c => c.Name == CardName.Temple);
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Temple);
            player.Wonder.BuildStage();
            player.Wonder.BuildStage();

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(new List<ITurnPlayer> { player }, new List<IStructureCard>());
            uow.Commit();
            Assert.IsTrue(player.Cards.Any(c => c.Name == CardName.Temple));
            Assert.AreEqual(ConstantValues.INITIAL_COINS, player.Coins);
        }

        [TestMethod]
        public void BuyCardUsingCommercialCardReourceTest()
        {
            var player = players[0];
            //Produces 1 wood and 1 commercial card that produces any raw material
            player.Cards.Add(cards.First(c => c.Name == CardName.Caravansery));
            player.ChosenAction = TurnAction.BuyCard;
            //Costs 1 glass
            player.SelectableCards[0] = cards.First(c => c.Name == CardName.Baths);
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Baths);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(new List<ITurnPlayer> { player }, new List<IStructureCard>());
            uow.Commit();

            Assert.IsTrue(player.Cards.Any(c => c.Name == CardName.Baths));
        }

        [TestMethod]
        public void TryBuyCardUsingChoosableResouceTwiceTest()
        {
            var player = players[2];
            //Produces 1 papyrus, 1 glass and 1 clay / wood            
            player.Cards.Add(cards.First(c => c.Name == CardName.TreeFarm));
            player.Cards.Add(cards.First(c => c.Name == CardName.Glassworks));
            player.ChosenAction = TurnAction.BuyCard;
            //Costs 1 wood, 1 clay and 1 glass
            player.SelectableCards[0] = cards.First(c => c.Name == CardName.Temple);
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Temple);

            players[0].SetWonder(WonderFactory.CreateWonder(WonderName.ColossusOfRhodes, WonderBoardSide.A));
            players[0].ChosenAction = TurnAction.SellCard;
            players[0].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            players[1].ChosenAction = TurnAction.SellCard;
            players[1].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();
            Assert.IsFalse(player.Cards.Any(c => c.Name == CardName.Temple));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, player.Coins);
            Assert.AreEqual(2, player.Cards.Count);
        }

        [TestMethod]
        public void TryBuyCardMissingResourceTest()
        {
            var player = players[0];
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);

            players[1].ChosenAction = TurnAction.SellCard;
            players[1].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            players[2].ChosenAction = TurnAction.SellCard;
            players[2].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();
            Assert.IsFalse(player.Cards.Any(c => c.Name == CardName.Caravansery));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, player.Coins);
        }

        [TestMethod]
        public void TryBuySameCardAgainTest()
        {
            var player = players[0];
            player.Cards.Add(cards.First(c => c.Name == CardName.Caravansery));
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(new List<ITurnPlayer> { player }, new List<IStructureCard>());
            uow.Commit();

            Assert.AreEqual(1, player.Cards.Count(c => c.Name == CardName.Caravansery));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, player.Coins);
        }

        [TestMethod]
        public void BuyCardBorrowingResources()
        {
            var extraCoins = 5;
            players[0].ReceiveCoin(extraCoins);
            players[0].InitializeTurnData();
            //Player produces 2 woods
            //Buy card costing 2 clays and 1 papyrus
            players[0].Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            players[0].ChosenAction = TurnAction.BuyCard;
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);
            players[0].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Clay });
            players[0].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Clay });
            players[0].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Papyrus });

            //Produces 2 clays and 1 glass
            players[1].Cards.Add(cards.First(c => c.Name == CardName.Brickyard));
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();
            Assert.IsTrue(players[0].Cards.Any(c => c.Name == CardName.Laboratory));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + extraCoins - (3 * ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT), players[0].Coins);
            Assert.IsTrue(players[1].GetResourcesAvailable(true).Any(r => r == ResourceType.Clay));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + (2 * ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT), players[1].Coins);
            Assert.IsTrue(players[2].GetResourcesAvailable(true).Any(r => r == ResourceType.Papyrus));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT, players[2].Coins);
        }

        [TestMethod]
        public void BuyCardBorrowingAllResources()
        {
            players[0].InitializeTurnData();
            //Player produces 2 woods
            //Buy card costing 1 loom
            players[0].SelectableCards[0] = cards.First(c => c.Name == CardName.Apothecary);
            players[0].Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            players[0].ChosenAction = TurnAction.BuyCard;
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.Apothecary);
            players[0].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Loom });

            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus and 1 loom
            players[2].Cards.Add(cards.First(c => c.Name == CardName.Loom));
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();
            Assert.IsTrue(players[0].Cards.Any(c => c.Name == CardName.Apothecary));
            Assert.AreEqual(ConstantValues.INITIAL_COINS - ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT, players[0].Coins);
            Assert.IsTrue(players[2].GetResourcesAvailable(true).Any(r => r == ResourceType.Loom));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT, players[2].Coins);
        }

        [TestMethod]
        public void BuyCardBorrowingChoosableResource()
        {
            var extraCoins = 5;
            players[0].ReceiveCoin(extraCoins);
            players[0].InitializeTurnData();
            //Player produces 2 woods
            //Buy card costing 2 clays and 1 papyrus
            players[0].ChosenAction = TurnAction.BuyCard;
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);
            players[0].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Clay });
            players[0].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Clay });
            players[0].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Papyrus });

            //Produces 1 clay, 1 glass and 1 clay/wood
            players[1].Cards.Add(cards.First(c => c.Name == CardName.ClayPool));
            players[1].Cards.Add(cards.First(c => c.Name == CardName.TreeFarm));
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();
            Assert.IsTrue(players[0].Cards.Any(c => c.Name == CardName.Laboratory));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + extraCoins - (3 * ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT), players[0].Coins);
            Assert.IsTrue(players[1].GetResourcesAvailable(true).Any(r => r == ResourceType.Clay));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + (2 * ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT), players[1].Coins);
            Assert.IsTrue(players[2].GetResourcesAvailable(true).Any(r => r == ResourceType.Papyrus));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT, players[2].Coins);
        }

        [TestMethod]
        public void BuyCardBorrowingResourcesWithDiscountToRight()
        {
            var extraCoins = 5;
            //Player produces 1 wood
            //Buy card costing 1 stone
            players[0].Cards.Add(cards.First(c => c.Name == CardName.WestTradingPost));
            players[0].ChosenAction = TurnAction.BuyCard;
            players[0].SelectableCards[0] = cards.First(c => c.Name == CardName.Baths);
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.Baths);
            players[0].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Stone });
            players[0].ReceiveCoin(extraCoins);

            //Produces 1 stone, 1 glass
            players[1].Cards.Add(cards.First(c => c.Name == CardName.StonePit));
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();

            Assert.IsTrue(players[0].Cards.Any(c => c.Name == CardName.Baths));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + extraCoins - ConstantValues.COIN_VALUE_FOR_SHARE_DISCOUNT, players[0].Coins);
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.COIN_VALUE_FOR_SHARE_DISCOUNT, players[1].Coins);
        }

        [TestMethod]
        public void TryBorrowingResourcesCannotPayTest()
        {
            players[0].PayCoin(ConstantValues.INITIAL_COINS);
            players[0].InitializeTurnData();
            //Player produces 1 wood
            //Buy card costing 1 stone
            players[0].ChosenAction = TurnAction.BuyCard;
            players[0].SelectableCards[0] = cards.First(c => c.Name == CardName.Baths);
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.Baths);
            players[0].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Stone });

            //Produces 1 stone, 1 glass
            players[1].Cards.Add(cards.First(c => c.Name == CardName.StonePit));
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();

            Assert.IsFalse(players[0].Cards.Any(c => c.Name == CardName.Baths));
            Assert.AreEqual(ConstantValues.SELL_CARD_COINS, players[0].Coins);
            Assert.AreEqual(ConstantValues.INITIAL_COINS, players[1].Coins);
        }

        [TestMethod]
        public void TryBorrowingResourcesPlayedThisTurnTest()
        {
            //Player produces 1 wood
            players[0].ChosenAction = TurnAction.BuyCard;
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 glass
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectableCards[0] = cards.First(c => c.Name == CardName.Barracks);
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.Barracks);
            players[1].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Ore });

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();

            Assert.IsFalse(players[1].Cards.Any(c => c.Name == CardName.Barracks));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, players[1].Coins);
            Assert.AreEqual(ConstantValues.INITIAL_COINS, players[0].Coins);
        }

        [TestMethod]
        public void TryBorrowingResourcesFromCommercialCardsFailsTest()
        {
            //Player produces 1 wood and 1 commercial card that produces any raw material
            players[0].Cards.Add(cards.FirstOrDefault(c => c.Name == CardName.Caravansery));
            players[0].ChosenAction = TurnAction.BuyCard;
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 glass
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectableCards[0] = cards.First(c => c.Name == CardName.Barracks);
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.Barracks);
            players[1].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Ore });

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();

            Assert.IsFalse(players[1].Cards.Any(c => c.Name == CardName.Barracks));
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, players[1].Coins);
            Assert.AreEqual(ConstantValues.INITIAL_COINS, players[0].Coins);
        }

        [TestMethod]
        public void BuildStageTest()
        {
            var player = players[0];
            player.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.ChosenAction = TurnAction.BuildWonderStage;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(new List<ITurnPlayer> { player }, new List<IStructureCard>());
            uow.Commit();

            Assert.AreEqual(1, player.Wonder.StagesBuilt);
            Assert.AreEqual(ConstantValues.INITIAL_COINS, player.Coins);
        }

        [TestMethod]
        public void TryBuildStageMissingResourcesTest()
        {
            var player = players[0];
            player.ChosenAction = TurnAction.BuildWonderStage;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);

            players[1].ChosenAction = TurnAction.SellCard;
            players[1].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            players[2].ChosenAction = TurnAction.SellCard;
            players[2].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();

            Assert.AreEqual(0, player.Wonder.StagesBuilt);
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, player.Coins);
        }

        [TestMethod]
        public void BuildStageBorrowingResourcesFromLeft()
        {
            //Player produces 1 wood
            //Build wonder stage costing 2 woods            
            players[0].ChosenAction = TurnAction.BuildWonderStage;
            players[0].SelectedCard = players[0].SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);
            players[0].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Wood });

            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus and 1 wood
            players[2].Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.II, 1);
            manager.Play(players, new List<IStructureCard>());
            uow.Commit();
                        
            Assert.AreEqual(ConstantValues.INITIAL_COINS - ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT, players[0].Coins);
            Assert.AreEqual(ConstantValues.INITIAL_COINS + ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT, players[2].Coins);
        }

        [TestMethod]
        public void PlaySeventhCardGetRewardsWrongTurnTest()
        {
            players[0].ExecutedAction = TurnAction.SellCard;
            players[1].ExecutedAction = TurnAction.SellCard;
            players[2].ExecutedAction = TurnAction.SellCard;

            var player = players[0];
            player.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.Cards.Add(cards.First(c => c.Name == CardName.TimberYard));
            player.SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.B));
            player.Wonder.BuildStage();
            player.Wonder.BuildStage();

            //Set to only when card left to choose
            var chosenCard = player.SelectableCards.First();
            player.SetSelectableCards(new List<IStructureCard> { chosenCard });
            player.ChosenAction = TurnAction.BuyCard;

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.I, 4);
            manager.GetRewards(players, new List<IStructureCard>());
            uow.Commit();

            Assert.IsFalse(player.Cards.Any(c => c.Name == chosenCard.Name));
        }

        [TestMethod]
        public void PlaySeventhCardGetRewardsTest()
        {
            players[0].ExecutedAction = TurnAction.SellCard;
            players[1].ExecutedAction = TurnAction.SellCard;
            players[2].ExecutedAction = TurnAction.SellCard;

            var player = players[0];
            player.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.Cards.Add(cards.First(c => c.Name == CardName.TimberYard));
            player.SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.B));
            player.Wonder.BuildStage();
            player.Wonder.BuildStage();

            //Set to only when card left to choose
            var chosenCard = player.SelectableCards.First();
            player.SetSelectableCards(new List<IStructureCard> { chosenCard });
            player.ChosenAction = TurnAction.BuyCard;

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.I, 6);
            manager.GetMultipleTimesRewards(players, new List<IStructureCard>());
            uow.Commit();

            Assert.IsTrue(player.Cards.Any(c => c.Name == chosenCard.Name));
        }

        [TestMethod]
        public void CoinPerRawMaterialCardGetRewardsTest()
        {
            players[0].ExecutedAction = TurnAction.BuyCard;
            players[1].ExecutedAction = TurnAction.SellCard;
            players[2].ExecutedAction = TurnAction.SellCard;

            var player = players[0];
            player.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.Cards.Add(cards.First(c => c.Name == CardName.TimberYard));

            var player2 = players[1];
            player2.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));

            var player3 = players[2];
            player3.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player3.Cards.Add(cards.First(c => c.Name == CardName.TimberYard));

            //Set to only when card left to choose
            player.SetSelectableCards(cards.Where(c => c.Name == CardName.Vineyard).ToList());
            player.SelectedCard = player.SelectableCards.First();            
            player.ChosenAction = TurnAction.BuyCard;

            var expectedCoins = player.Coins + 5;

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.I, 4);
            manager.GetRewards(players, new List<IStructureCard>());
            uow.Commit();

            Assert.AreEqual(expectedCoins, player.Coins);
        }

        [TestMethod]
        public void CoinPerWonderStageBuiltCardGetRewardsTest()
        {
            players[0].ExecutedAction = TurnAction.BuyCard;
            players[1].ExecutedAction = TurnAction.SellCard;
            players[2].ExecutedAction = TurnAction.SellCard;

            var player = players[0];
            player.Wonder.BuildStage();
            player.Wonder.BuildStage();

            //Set to only when card left to choose
            player.SetSelectableCards(cards.Where(c => c.Name == CardName.Arena).ToList());
            player.SelectedCard = player.SelectableCards.First();
            player.ChosenAction = TurnAction.BuyCard;

            var expectedCoins = player.Coins + 6;

            var uow = new UnitOfWork();
            var manager = new TurnManager(uow);
            manager.SetCurrentInfo(Age.I, 4);
            manager.GetRewards(players, new List<IStructureCard>());
            uow.Commit();

            Assert.AreEqual(expectedCoins, player.Coins);
        }
    }
}
