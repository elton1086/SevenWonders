using AutoFixture;
using AutoFixture.Xunit2;
using Moq;
using SevenWonders.BaseEntities;
using SevenWonders.CardGenerator;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Extensions;
using SevenWonders.Factories;
using SevenWonders.Services;
using SevenWonders.Services.Contracts;
using SevenWonders.Utilities;
using SevenWonders.XUnit.Test.AutoData;
using SevenWonders.XUnit.Test.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SevenWonders.UnitTest.Services
{
    public class TurnManagerTest
    {
        List<StructureCard> cards;

        public TurnManagerTest()
        {
            //players = new List<TurnPlayer>
            //{
            //    new TurnPlayer("jennifer"),
            //    new TurnPlayer("jessica"),
            //    new TurnPlayer("amanda")
            //};

            //CreateCards();

            //var caravansery = cards.First(c => c.Name == CardName.Caravansery);
            //var foundry = cards.First(c => c.Name == CardName.Foundry);
            //var laboratory = cards.First(c => c.Name == CardName.Laboratory);
            //var oreVein = cards.First(c => c.Name == CardName.OreVein);

            //#region Player 1
            //var player1 = player;
            //player1.SetSelectableCards(new List<StructureCard>
            //{
            //    caravansery,
            //    foundry,
            //    laboratory,
            //    oreVein
            //});
            //player1.SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.A));
            //player1.ReceiveCoin(ConstantValues.INITIAL_COINS);
            //player1.ResetData();
            //#endregion

            //#region Player 2
            //players[1].SetSelectableCards(new List<StructureCard>
            //{
            //    foundry,
            //    caravansery,
            //    laboratory,
            //    oreVein
            //});
            //players[1].SetWonder(WonderFactory.CreateWonder(WonderName.LighthouseOfAlexandria, WonderBoardSide.A));
            //players[1].ReceiveCoin(ConstantValues.INITIAL_COINS);
            //players[1].ResetData();
            //#endregion

            //#region Player 3
            //players[2].SetSelectableCards(new List<StructureCard>
            //{
            //    laboratory,
            //    caravansery,
            //    foundry,
            //    oreVein
            //});
            //players[2].SetWonder(WonderFactory.CreateWonder(WonderName.TempleOfArthemisInEphesus, WonderBoardSide.A));
            //players[2].ReceiveCoin(ConstantValues.INITIAL_COINS);
            //players[2].ResetData();
            //#endregion
        }

        [Theory, AutoBaseGameData(3)]
        public void MoveSelectableCardsToLeftTest(List<TurnPlayer> players)
        {
            var cardOne = players[0].SelectableCards[0].Name;
            var cardTwo = players[1].SelectableCards[0].Name;
            var cardThree = players[2].SelectableCards[0].Name;

            players.MoveSelectableCards(Age.I);

            Assert.Equal(cardOne, players[1].SelectableCards[0].Name);
            Assert.Equal(cardTwo, players[2].SelectableCards[0].Name);
            Assert.Equal(cardThree, players[0].SelectableCards[0].Name);
        }

        [Theory, AutoGameSetupData(3)]
        public void MoveSelectableCardsToRightTest(List<TurnPlayer> players)
        {
            var cardOne = players[0].SelectableCards[0].Name;
            var cardTwo = players[1].SelectableCards[0].Name;
            var cardThree = players[2].SelectableCards[0].Name;
            players.MoveSelectableCards(Age.II);
            Assert.Equal(cardOne, players[2].SelectableCards[0].Name);
            Assert.Equal(cardTwo, players[0].SelectableCards[0].Name);
            Assert.Equal(cardThree, players[1].SelectableCards[0].Name);
        }

        [Theory, AutoBaseGameData(1)]
        public void SellCardTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            var selectedCard = player.SelectableCards.First();
            var expectedCoins = player.GamePlayer.Coins + ConstantValues.SELL_CARD_COINS;

            player.ChosenAction = TurnAction.SellCard;            
            player.SelectedCard = selectedCard;

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            var discarded = new List<StructureCard>();
            manager.Play(new List<TurnPlayer> { player }, discarded, Age.II);
            uow.Commit();
            Assert.Contains(selectedCard, discarded);
            Assert.Equal(expectedCoins, player.GamePlayer.Coins);
            Assert.DoesNotContain(selectedCard, player.GamePlayer.Cards);
        }

        [Theory, AutoBaseGameData(1)]
        public void BuyCardPayCoin([Frozen]Mock<ITradeManager> tradeManager, TurnManager manager, List<TurnPlayer> players, List<StructureCard> cards)
        {
            var player = players.First();
            player.GamePlayer.ReceiveCoin(1);

            var selectedCard = cards.First(c => c.Name == CardName.TimberYard);
            var expectedCoins = player.GamePlayer.Coins - 1;

            player.ChosenAction = TurnAction.BuyCard;            
            player.SelectableCards[0] = selectedCard;
            player.SelectedCard = selectedCard;

            tradeManager.Setup(m => m.BorrowResources(player, It.IsAny<GamePlayer>(), It.IsAny<GamePlayer>(), It.IsAny<IList<BorrowResourceData>>(), true)).Returns(true);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.Contains(selectedCard, player.GamePlayer.Cards);
            Assert.Equal(expectedCoins, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(1)]
        public void BuyCardWithOwnResourcesTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(new List<TurnPlayer> { player }, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.Contains(player.GamePlayer.Cards, c => c.Name == CardName.Caravansery);
            Assert.Equal(ConstantValues.INITIAL_COINS, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(1)]
        public void BuyCardForFreeTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.Workshop));
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(new List<TurnPlayer> { player }, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.Contains(player.GamePlayer.Cards, c => c.Name == CardName.Laboratory);
        }

        [Theory, AutoGameSetupData(1)]
        public void BuyCardUsingSpecialCaseTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);
            player.GamePlayer.Wonder.BuildStage();
            player.GamePlayer.Wonder.BuildStage();
            player.SpecialCaseToUse = SpecialCaseType.PlayCardForFreeOncePerAge;

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(new List<TurnPlayer> { player }, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.Contains(player.GamePlayer.Cards, c => c.Name == CardName.Laboratory);
            Assert.Equal(Age.II, (Age)player.GamePlayer.GetNonResourceEffects().First(e => e.Type == EffectType.PlayCardForFreeOncePerAge).Info);
        }

        [Theory, AutoGameSetupData(1)]
        public void BuyCardUsingChoosableResouceTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            //Produces 1 wood and 1 clay / wood
            //Costs 2 woods
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.TreeFarm));
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(new List<TurnPlayer> { player }, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.Contains(player.GamePlayer.Cards, c => c.Name == CardName.Caravansery);
            Assert.Equal(ConstantValues.INITIAL_COINS, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(2)]
        public void BuyCardUsingChoosableResouceFromWonderStageTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players[1];
            //Produces 1 glass and 1 clay / wood and 1 of any raw material (2nd stage)      
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.TreeFarm));
            player.ChosenAction = TurnAction.BuyCard;
            //Costs 1 wood, 1 clay and 1 glass
            player.SelectableCards[0] = cards.First(c => c.Name == CardName.Temple);
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Temple);
            player.GamePlayer.Wonder.BuildStage();
            player.GamePlayer.Wonder.BuildStage();

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(new List<TurnPlayer> { player }, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.Contains(player.GamePlayer.Cards, c => c.Name == CardName.Temple);
            Assert.Equal(ConstantValues.INITIAL_COINS, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(1)]
        public void BuyCardUsingCommercialCardReourceTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            //Produces 1 wood and 1 commercial card that produces any raw material
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.Caravansery));
            player.ChosenAction = TurnAction.BuyCard;
            //Costs 1 glass
            player.SelectableCards[0] = cards.First(c => c.Name == CardName.Baths);
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Baths);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(new List<TurnPlayer> { player }, new List<StructureCard>(), Age.II);
            uow.Commit();

            Assert.Contains(player.GamePlayer.Cards, c => c.Name == CardName.Baths);
        }

        [Theory, AutoGameSetupData(3)]
        public void TryBuyCardUsingChoosableResouceTwiceTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players[2];
            //Produces 1 papyrus, 1 glass and 1 clay / wood            
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.TreeFarm));
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.Glassworks));
            player.ChosenAction = TurnAction.BuyCard;
            //Costs 1 wood, 1 clay and 1 glass
            player.SelectableCards[0] = cards.First(c => c.Name == CardName.Temple);
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Temple);

            player.GamePlayer.SetWonder(WonderFactory.CreateWonder(WonderName.ColossusOfRhodes, WonderBoardSide.A));
            player.ChosenAction = TurnAction.SellCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            players[1].ChosenAction = TurnAction.SellCard;
            players[1].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.DoesNotContain(player.GamePlayer.Cards, c => c.Name == CardName.Temple);
            Assert.Equal(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, player.GamePlayer.Coins);
            Assert.Equal(2, player.GamePlayer.Cards.Count);
        }

        [Theory, AutoGameSetupData(3)]
        public void TryBuyCardMissingResourceTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);

            players[1].ChosenAction = TurnAction.SellCard;
            players[1].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            players[2].ChosenAction = TurnAction.SellCard;
            players[2].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.DoesNotContain(player.GamePlayer.Cards, c => c.Name == CardName.Caravansery);
            Assert.Equal(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(1)]
        public void TryBuySameCardAgainTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.Caravansery));
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(new List<TurnPlayer> { player }, new List<StructureCard>(), Age.II);
            uow.Commit();

            Assert.Equal(1, player.GamePlayer.Cards.Count(c => c.Name == CardName.Caravansery));
            Assert.Equal(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void BuyCardBorrowingResources(TurnManager manager, List<TurnPlayer> players)
        {
            var extraCoins = 5;
            var player = players.First();
            player.GamePlayer.ReceiveCoin(extraCoins);
            player.ResetData();
            //Player produces 2 woods
            //Buy card costing 2 clays and 1 papyrus
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);
            player.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Clay });
            player.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Clay });
            player.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Papyrus });

            //Produces 2 clays and 1 glass
            players[1].GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.Brickyard));
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.Contains(player.GamePlayer.Cards, c => c.Name == CardName.Laboratory);
            Assert.Equal(ConstantValues.INITIAL_COINS + extraCoins - (3 * ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT), player.GamePlayer.Coins);
            Assert.Contains(players[1].GetResourcesAvailable(true), r => r == ResourceType.Clay);
            Assert.Equal(ConstantValues.INITIAL_COINS + (2 * ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT), players[1].GamePlayer.Coins);
            Assert.Contains(players[2].GetResourcesAvailable(true), r => r == ResourceType.Papyrus);
            Assert.Equal(ConstantValues.INITIAL_COINS + ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT, players[2].GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void BuyCardBorrowingAllResources(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            player.ResetData();
            //Player produces 2 woods
            //Buy card costing 1 loom
            player.SelectableCards[0] = cards.First(c => c.Name == CardName.Apothecary);
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Apothecary);
            player.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Loom });

            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus and 1 loom
            players[2].GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.Loom));
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.Contains(player.GamePlayer.Cards, c => c.Name == CardName.Apothecary);
            Assert.Equal(ConstantValues.INITIAL_COINS - ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT, player.GamePlayer.Coins);
            Assert.Contains(players[2].GetResourcesAvailable(true), r => r == ResourceType.Loom);
            Assert.Equal(ConstantValues.INITIAL_COINS + ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT, players[2].GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void BuyCardBorrowingChoosableResource(TurnManager manager, List<TurnPlayer> players)
        {
            var extraCoins = 5;
            var player = players.First();
            player.GamePlayer.ReceiveCoin(extraCoins);
            player.ResetData();
            //Player produces 2 woods
            //Buy card costing 2 clays and 1 papyrus
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);
            player.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Clay });
            player.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Clay });
            player.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Papyrus });

            //Produces 1 clay, 1 glass and 1 clay/wood
            players[1].GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.ClayPool));
            players[1].GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.TreeFarm));
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();
            Assert.Contains(player.GamePlayer.Cards, c => c.Name == CardName.Laboratory);
            Assert.Equal(ConstantValues.INITIAL_COINS + extraCoins - (3 * ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT), player.GamePlayer.Coins);
            Assert.Contains(players[1].GetResourcesAvailable(true), r => r == ResourceType.Clay);
            Assert.Equal(ConstantValues.INITIAL_COINS + (2 * ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT), players[1].GamePlayer.Coins);
            Assert.Contains(players[2].GetResourcesAvailable(true), r => r == ResourceType.Papyrus);
            Assert.Equal(ConstantValues.INITIAL_COINS + ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT, players[2].GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void BuyCardBorrowingResourcesWithDiscountToRight(TurnManager manager, List<TurnPlayer> players)
        {
            var extraCoins = 5;
            var player = players.First();
            //Player produces 1 wood
            //Buy card costing 1 stone
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.WestTradingPost));
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectableCards[0] = cards.First(c => c.Name == CardName.Baths);
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Baths);
            player.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Stone });
            player.GamePlayer.ReceiveCoin(extraCoins);

            //Produces 1 stone, 1 glass
            players[1].GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.StonePit));
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();

            Assert.Contains(player.GamePlayer.Cards, c => c.Name == CardName.Baths);
            Assert.Equal(ConstantValues.INITIAL_COINS + extraCoins - ConstantValues.COIN_VALUE_FOR_SHARE_DISCOUNT, player.GamePlayer.Coins);
            Assert.Equal(ConstantValues.INITIAL_COINS + ConstantValues.COIN_VALUE_FOR_SHARE_DISCOUNT, players[1].GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void TryBorrowingResourcesCannotPayTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();

            player.GamePlayer.PayCoin(ConstantValues.INITIAL_COINS);
            player.ResetData();
            //Player produces 1 wood
            //Buy card costing 1 stone
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectableCards[0] = cards.First(c => c.Name == CardName.Baths);
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Baths);
            player.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = ResourceType.Stone });

            //Produces 1 stone, 1 glass
            players[1].GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.StonePit));
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();

            Assert.DoesNotContain(player.GamePlayer.Cards, c => c.Name == CardName.Baths);
            Assert.Equal(ConstantValues.SELL_CARD_COINS, player.GamePlayer.Coins);
            Assert.Equal(ConstantValues.INITIAL_COINS, players[1].GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void TryBorrowingResourcesPlayedThisTurnTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();

            //Player produces 1 wood
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 glass
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectableCards[0] = cards.First(c => c.Name == CardName.Barracks);
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.Barracks);
            players[1].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Ore });

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();

            Assert.DoesNotContain(players[1].GamePlayer.Cards, c => c.Name == CardName.Barracks);
            Assert.Equal(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, players[1].GamePlayer.Coins);
            Assert.Equal(ConstantValues.INITIAL_COINS, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void TryBorrowingResourcesFromCommercialCardsFailsTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();

            //Player produces 1 wood and 1 commercial card that produces any raw material
            player.GamePlayer.Cards.Add(cards.FirstOrDefault(c => c.Name == CardName.Caravansery));
            player.ChosenAction = TurnAction.BuyCard;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            //Produces 1 glass
            players[1].ChosenAction = TurnAction.BuyCard;
            players[1].SelectableCards[0] = cards.First(c => c.Name == CardName.Barracks);
            players[1].SelectedCard = players[1].SelectableCards.FirstOrDefault(c => c.Name == CardName.Barracks);
            players[1].ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Ore });

            //Produces 1 papyrus
            players[2].ChosenAction = TurnAction.BuyCard;
            players[2].SelectedCard = players[2].SelectableCards.FirstOrDefault(c => c.Name == CardName.OreVein);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();

            Assert.DoesNotContain(players[1].GamePlayer.Cards, c => c.Name == CardName.Barracks);
            Assert.Equal(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, players[1].GamePlayer.Coins);
            Assert.Equal(ConstantValues.INITIAL_COINS, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(1)]
        public void BuildStageTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.ChosenAction = TurnAction.BuildWonderStage;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(new List<TurnPlayer> { player }, new List<StructureCard>(), Age.II);
            uow.Commit();

            Assert.Equal(1, player.GamePlayer.Wonder.StagesBuilt);
            Assert.Equal(ConstantValues.INITIAL_COINS, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void TryBuildStageMissingResourcesTest(TurnManager manager, List<TurnPlayer> players)
        {
            var player = players.First();
            player.ChosenAction = TurnAction.BuildWonderStage;
            player.SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Laboratory);

            players[1].ChosenAction = TurnAction.SellCard;
            players[1].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            players[2].ChosenAction = TurnAction.SellCard;
            players[2].SelectedCard = player.SelectableCards.FirstOrDefault(c => c.Name == CardName.Caravansery);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();

            Assert.Equal(0, player.GamePlayer.Wonder.StagesBuilt);
            Assert.Equal(ConstantValues.INITIAL_COINS + ConstantValues.SELL_CARD_COINS, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void BuildStageBorrowingResourcesFromLeft([Frozen]IFixture fixture, [Frozen]Mock<ITradeManager> tradeManager,
            TurnManager manager, List<TurnPlayer> players)
        {
            //Player produces 1 wood and has 2 coins
            //Build wonder stage costing 2 woods                   
            var wonder1 = fixture.Build<MockWonder>()
                .With(w => w.UseResource, ResourceType.Wood)
                .Create();
            wonder1.SetStages(new List<WonderStage> { new WonderStage(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood }, null) });
            var player1 = players[0];
            player1.GamePlayer.SetWonder(wonder1);
            player1.GamePlayer.ReceiveCoin(2);
            player1.SetSelectableCards(fixture.CreateMany<MockCard>(1).ToList<StructureCard>());
            player1.ChosenAction = TurnAction.BuildWonderStage;
            player1.SelectedCard = player1.SelectableCards.First();
            player1.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = ResourceType.Wood });

            var player2 = players[1];
            player2.ChosenAction = TurnAction.SellCard;
            player2.SetSelectableCards(fixture.CreateMany<MockCard>(1).ToList<StructureCard>());
            player2.SelectedCard = player2.SelectableCards.First();

            //Produces 1 wood
            var player3 = players[2];
            player3.GamePlayer.Cards.Add(fixture.Build<MockCard>()
                .With(c => c.SetEffects, new List<Effect> { new Effect(EffectType.Wood) })
                .Create());
            player3.ChosenAction = TurnAction.SellCard;
            player3.SetSelectableCards(fixture.CreateMany<MockCard>(1).ToList<StructureCard>());
            player3.SelectedCard = player3.SelectableCards.First();

            tradeManager.Setup(m => m.BorrowResources(player1, player3.GamePlayer, player2.GamePlayer, It.IsAny<IList<BorrowResourceData>>(), It.IsAny<bool>()))
                .Callback(() => player1.TemporaryResources.AddRange(new List<ResourceType> { ResourceType.Wood }))
                .Returns(true);

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.Play(players, new List<StructureCard>(), Age.II);
            uow.Commit();

            Assert.Equal(0, player1.GamePlayer.Coins);
            Assert.Equal(5, player3.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void PlaySeventhCardGetRewardsWrongTurnTest(TurnManager manager, List<TurnPlayer> players)
        {
            players[0].ExecutedAction = TurnAction.SellCard;
            players[1].ExecutedAction = TurnAction.SellCard;
            players[2].ExecutedAction = TurnAction.SellCard;

            var player = players.First();
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.TimberYard));
            player.GamePlayer.SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.B));
            player.GamePlayer.Wonder.BuildStage();
            player.GamePlayer.Wonder.BuildStage();

            //Set to only when card left to choose
            var chosenCard = player.SelectableCards.First();
            player.SetSelectableCards(new List<StructureCard> { chosenCard });
            player.ChosenAction = TurnAction.BuyCard;

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.GetRewards(players, new List<StructureCard>(), Age.I);
            uow.Commit();

            Assert.DoesNotContain(player.GamePlayer.Cards, c => c.Name == chosenCard.Name);
        }

        [Theory, AutoGameSetupData(3)]
        public void PlaySeventhCardGetRewardsTest(TurnManager manager, List<TurnPlayer> players)
        {
            players[0].ExecutedAction = TurnAction.SellCard;
            players[1].ExecutedAction = TurnAction.SellCard;
            players[2].ExecutedAction = TurnAction.SellCard;

            var player = players.First();
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.TimberYard));
            player.GamePlayer.SetWonder(WonderFactory.CreateWonder(WonderName.HangingGardensOfBabylon, WonderBoardSide.B));
            player.GamePlayer.Wonder.BuildStage();
            player.GamePlayer.Wonder.BuildStage();

            //Set to only when card left to choose
            var chosenCard = player.SelectableCards.First();
            player.SetSelectableCards(new List<StructureCard> { chosenCard });
            player.ChosenAction = TurnAction.BuyCard;

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.GetMultipleTimesRewards(players, new List<StructureCard>(), 6, Age.I);
            uow.Commit();

            Assert.Contains(player.GamePlayer.Cards, c => c.Name == chosenCard.Name);
        }

        [Theory, AutoGameSetupData(3)]
        public void CoinPerRawMaterialCardGetRewardsTest(TurnManager manager, List<TurnPlayer> players)
        {
            players[0].ExecutedAction = TurnAction.BuyCard;
            players[1].ExecutedAction = TurnAction.SellCard;
            players[2].ExecutedAction = TurnAction.SellCard;

            var player = players.First();
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.TimberYard));

            var player2 = players[1];
            player2.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));

            var player3 = players[2];
            player3.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.LumberYard));
            player3.GamePlayer.Cards.Add(cards.First(c => c.Name == CardName.TimberYard));

            //Set to only when card left to choose
            player.SetSelectableCards(cards.Where(c => c.Name == CardName.Vineyard).ToList());
            player.SelectedCard = player.SelectableCards.First();
            player.ChosenAction = TurnAction.BuyCard;

            var expectedCoins = player.GamePlayer.Coins + 5;

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.GetRewards(players, new List<StructureCard>(), Age.I);
            uow.Commit();

            Assert.Equal(expectedCoins, player.GamePlayer.Coins);
        }

        [Theory, AutoGameSetupData(3)]
        public void CoinPerWonderStageBuiltCardGetRewardsTest(TurnManager manager, List<TurnPlayer> players)
        {
            players[0].ExecutedAction = TurnAction.BuyCard;
            players[1].ExecutedAction = TurnAction.SellCard;
            players[2].ExecutedAction = TurnAction.SellCard;

            var player = players.First();
            player.GamePlayer.Wonder.BuildStage();
            player.GamePlayer.Wonder.BuildStage();

            //Set to only when card left to choose
            player.SetSelectableCards(cards.Where(c => c.Name == CardName.Arena).ToList());
            player.SelectedCard = player.SelectableCards.First();
            player.ChosenAction = TurnAction.BuyCard;

            var expectedCoins = player.GamePlayer.Coins + 6;

            var uow = new UnitOfWork();
            manager.SetScope(uow);
            manager.GetRewards(players, new List<StructureCard>(), Age.I);
            uow.Commit();

            Assert.Equal(expectedCoins, player.GamePlayer.Coins);
        }

        //[Theory, AutoBaseGameSetupData(1)]
        //public void ComputeCopyGuildCardTest(TurnManager manager, List<TurnPlayer> players)
        //{
        //    var cardName = CardName.SpiesGuild;
        //    var guild = new GuildCard(cardName, 3, Age.III, null, null, new List<Effect> { new Effect(EffectType.VictoryPointPerMilitaryCard, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) });
        //    manager.CreateNewPlayer("ashley");
        //    manager.CreateNewPlayer("kate");
        //    manager.SetupGame();
        //    var player1 = manager.Players[0];
        //    var player2 = manager.Players[1];

        //    player1.SetWonder(WonderFactory.CreateWonder(WonderName.StatueOfZeusInOlimpia, WonderBoardSide.B));
        //    player1.Wonder.BuildStage();
        //    player1.Wonder.BuildStage();
        //    player1.Wonder.BuildStage();
        //    player1.Wonder.EffectsAvailable.First(e => e.Type == EffectType.CopyGuildFromNeighbor).Info = cardName;

        //    player2.Cards.Add(guild);

        //    manager.GetPostGameRewards();

        //    Assert.Contains(player1.Cards, c => c.Name == cardName);
        //}

        private IEnumerable<StructureCard> CreateCards()
        {
            return new List<StructureCard>
            {
                new CommercialCard(CardName.Caravansery, 3, Age.II, new List<ResourceType>{ ResourceType.Wood, ResourceType.Wood }, new List<CardName> { CardName.Marketplace }, new List<Effect> { new Effect(EffectType.Clay), new Effect(EffectType.Wood), new Effect(EffectType.Stone), new Effect(EffectType.Ore) }),
                new RawMaterialCard(CardName.LumberYard, 4, Age.I, null, null, new List<Effect> { new Effect(EffectType.Wood) }),
                new RawMaterialCard(CardName.TimberYard, 3, Age.I, new List<ResourceType>{ ResourceType.Coin }, null, new List<Effect> { new Effect(EffectType.Stone), new Effect(EffectType.Wood) }),
                new RawMaterialCard(CardName.Foundry, 3, Age.II, new List<ResourceType>{ ResourceType.Coin }, null, new List<Effect> { new Effect(EffectType.Ore), new Effect(EffectType.Ore) }),
                new ScientificCard(CardName.Workshop, 3, Age.I, new List<ResourceType>{ ResourceType.Glass }, null, new List<Effect> { new Effect(EffectType.Gear) }),
                new ScientificCard(CardName.Laboratory, 3, Age.II, new List<ResourceType>{ ResourceType.Clay, ResourceType.Clay, ResourceType.Papyrus }, new List<CardName> { CardName.Workshop}, new List<Effect> { new Effect(EffectType.Gear) }),
                new RawMaterialCard(CardName.TreeFarm, 6, Age.I, new List<ResourceType>{ ResourceType.Coin }, null, new List<Effect> { new Effect(EffectType.Clay), new Effect(EffectType.Wood) }),
                new CivilianCard(CardName.Temple, 3, Age.II, new List<ResourceType>{ ResourceType.Clay, ResourceType.Wood, ResourceType.Glass }, new List<CardName> { CardName.Altar}, new List<Effect> { new Effect(EffectType.VictoryPoint, 3) }),
                new ManufacturedGoodCard(CardName.Glassworks, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.Glass) }),
                new RawMaterialCard(CardName.ClayPool, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.Clay) }),
                new RawMaterialCard(CardName.Brickyard, 3, Age.II, new List<ResourceType>{ ResourceType.Coin }, null, new List<Effect> { new Effect(EffectType.Clay, 2) }),
                new RawMaterialCard(CardName.OreVein, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.Ore) }),
                new CommercialCard(CardName.WestTradingPost, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.BuyRawMaterialDiscount, 1, PlayerDirection.ToTheLeft) }),
                new CivilianCard(CardName.Baths, 3, Age.I, new List<ResourceType>{ ResourceType.Stone }, null, new List<Effect> { new Effect(EffectType.VictoryPoint, 3) }),
                new RawMaterialCard(CardName.StonePit, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.Stone) }),
                new MilitaryCard(CardName.Barracks, 3, Age.I, new List<ResourceType>{ ResourceType.Ore }, null, new List<Effect> { new Effect(EffectType.Shield) }),
                new ScientificCard(CardName.Apothecary, 3, Age.I, new List<ResourceType>{ ResourceType.Loom }, null, new List<Effect> { new Effect(EffectType.Compass) }),
                new ManufacturedGoodCard(CardName.Loom, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.Loom) }),
                new CommercialCard(CardName.Vineyard, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.CoinPerRawMaterialCard, 1, PlayerDirection.Myself | PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) }),
                new CommercialCard(CardName.Arena, 3, Age.I, null, null, new List<Effect> { new Effect(EffectType.CoinPerWonderStageBuilt, 3, PlayerDirection.Myself) }),
            };
        }
    }
}
