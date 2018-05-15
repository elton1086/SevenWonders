using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Entities.Events;
using SevenWonder.Helper;
using SevenWonder.Services.Contracts;
using SevenWonder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonder.Services
{
    public class TurnManager : ITurnManager
    {
        IUnitOfWork unitOfWork;
        Age age;
        int turn;

        public TurnManager()
        {
            this.unitOfWork = new NullUnitOfWork();
        }

        public TurnManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void SetCurrentInfo(Age age, int turn)
        {
            this.age = age;
            this.turn = turn;
        }

        public void Play(IList<ITurnPlayer> players, IList<IStructureCard> discardedCards)
        {
            LoggerHelper.Info("Starting to play turn for all players");
            foreach (var player in players)
            {                
                var neighbors = NeighborsHelper.GetNeighbors(players.Select(p => (IPlayer)p).ToList(), player);
                Play(player, (IGamePlayer)neighbors[NeighborsHelper.RIGHTDIRECTION], (IGamePlayer)neighbors[NeighborsHelper.LEFTDIRECTION], discardedCards);
            }
        }

        private void Play(ITurnPlayer player, IGamePlayer rightPlayer, IGamePlayer leftPlayer, IList<IStructureCard> discardedCards)
        {
            LoggerHelper.DebugFormat("Player {0} will {1} {2}", player.Name, player.ChosenAction, player.SelectedCard.Name);
            var success = false;
            ITradeManager tradeManager = new TradeManager(unitOfWork);
            CheckResourcesToBorrow(player.SelectedCard, player);
            if ((player.ChosenAction != TurnAction.BuyCard || !player.HasCard(player.SelectedCard))
                && tradeManager.BorrowResources(player, rightPlayer, leftPlayer, player.ResourcesToBorrow, true))
            {
                switch (player.ChosenAction)
                {
                    case TurnAction.BuyCard:
                        success = BuyCard(player.SelectedCard, player, age, player.SpecialCaseToUse);
                        break;
                    case TurnAction.SellCard:
                        SellCard(player, discardedCards);
                        success = true;
                        break;
                    case TurnAction.BuildWonderStage:
                        success = BuildWonderStage(player);
                        break;
                    default:
                        break;
                }
            }

            if (!success)
            {
                LoggerHelper.DebugFormat("Player {0} could not {1}. Selling and discarding card", player.Name, player.ChosenAction);
                SellCard(player, discardedCards);
                player.ExecutedAction = TurnAction.SellCard;
            }
            else
            {
                player.ExecutedAction = player.ChosenAction;
            }
            RemoveCardFromSelectable(player);
        }

        public void MoveSelectableCards(IList<ITurnPlayer> players)
        {
            var direction = "to the left";
            switch (age)
            {
                case Age.I:
                    MoveCards(players);
                    break;
                case Age.II:
                    MoveCards(players.Reverse());
                    direction = "to the right";
                    break;
                case Age.III:
                    MoveCards(players);
                    break;
                default:
                    break;
            }
            LoggerHelper.DebugFormat("Moving all selectable cards {0}", direction);
        }

        private void MoveCards(IEnumerable<ITurnPlayer> players)
        {
            var cardsToMove = players.Last().SelectableCards;
            foreach (var currentPlayer in players)
            {
                var nextCards = currentPlayer.SelectableCards;
                currentPlayer.SetSelectableCards(cardsToMove);
                cardsToMove = nextCards;
            }
        }

        private void RemoveCardFromSelectable(ITurnPlayer player)
        {
            player.SelectableCards.Remove(player.SelectedCard);
        }

        #region Buy Card
        /// <summary>
        /// Buys the selected card.
        /// </summary>
        /// <param name="card">The selected card.</param>
        /// <param name="player">Player buying the card.</param>
        /// <param name="age">The age that is being played.</param>
        /// <param name="specialCase">The user wants to use a special case card.</param>
        /// <returns></returns>
        private bool BuyCard(IStructureCard card, ITurnPlayer player, Age age, SpecialCaseType specialCase = SpecialCaseType.None)
        {
            bool result = TryBuyCard(card, player, age, specialCase);
            if (result)
            {
                LoggerHelper.Debug("Card can be bought");
                unitOfWork.AddEvent(new AddCardEvent(player, card));
            }
            return result;
        }

        private bool TryBuyCard(IStructureCard card, ITurnPlayer player, Age age, SpecialCaseType specialCase)
        {
            if (CanGetForFree(card))
            {
                LoggerHelper.Debug("Card is free.");
                return true;
            }
            if (HasDemandedCard(card, player.Cards))
            {
                LoggerHelper.Debug("Player can build for free because of previous card");
                return true;
            }
            if (UseSpecialCase(player, specialCase, age))
            {
                LoggerHelper.DebugFormat("Player can play card for free, because of {0}", specialCase);
                return true;
            }

            var coins = card.ResourceCosts.Count(r => r == ResourceType.Coin);
            var resources = card.ResourceCosts.Where(r => r != ResourceType.Coin);
            LoggerHelper.DebugFormat("Structure card costs {0} coins and {1} resources", coins, resources.Count());
            if (player.CoinsLeft >= coins && !player.CheckResourceAvailability(resources.ToList(), false).Any())
            {                
                if (coins > 0)
                    unitOfWork.AddEvent(new PayCoinEvent(player, coins));
                return true;
            }

            return false;
        }

        private bool CanGetForFree(IStructureCard card)
        {
            //The card doesn't cost anything.
            if (!card.CardCosts.Any() && !card.ResourceCosts.Any())
                return true;
            return false;
        }

        private bool HasDemandedCard(IStructureCard card, IList<IStructureCard> currentCards)
        {
            bool result = false;
            foreach (var c in card.CardCosts)
            {
                //Player has the required card to be able to construct for free
                if (currentCards.Any(cc => cc.Name == c))
                {
                    result = true;
                    continue;
                }
            }
            return result;
        }

        #endregion

        #region Build Wonder Stage

        private bool BuildWonderStage(ITurnPlayer player)
        {
            bool result = TryBuildStage(player);
            if (result)
            {
                LoggerHelper.Debug("Wonder stage can be built");
                unitOfWork.AddEvent(new BuildStageEvent(player));
            }
            return result;
        }

        private bool TryBuildStage(ITurnPlayer player)
        {
            if (player.Wonder.NextStage == null)
                return false;
            var costs = player.Wonder.NextStage.Costs;
            var coins = costs.Count(r => r == ResourceType.Coin);
            var resources = costs.Where(r => r != ResourceType.Coin);
            if (player.CoinsLeft >= coins && !player.CheckResourceAvailability(resources.ToList(), false).Any())
            {
                if (coins > 0)
                    unitOfWork.AddEvent(new PayCoinEvent(player, coins));
                return true;
            }
            return false;
        }

        #endregion

        private void SellCard(ITurnPlayer player, IList<IStructureCard> discardedCards)
        {
            LoggerHelper.DebugFormat("Discard {0} and collect coins", player.SelectedCard.Name);
            unitOfWork.AddEvent(new DiscardCardEvent(player.SelectedCard, discardedCards));
            unitOfWork.AddEvent(new ReceiveCoinEvent(player, ConstantValues.SELL_CARD_COINS));
        }

        private bool UseSpecialCase(IGamePlayer player, SpecialCaseType specialCase, Age age)
        {
            bool result = false;
            var effect = ValidSpecialCase(player, specialCase, age);
            if (effect != null)
            {                
                unitOfWork.AddEvent(new EffectInfoEvent(effect, age));
                result = true;
            }
            return result;
        }

        private IEffect ValidSpecialCase(IGamePlayer player, SpecialCaseType specialCase, Age age)
        {
            IEffect result = null;
            if (specialCase == SpecialCaseType.None)
                return result;
            var effects = player.GetNonResourceEffects().Where(e => (int)specialCase == (int)e.Type).ToList();
            foreach (var e in effects)
            {
                switch (specialCase)
                {
                    //This is the only special case so far and it only works for building structure
                    case SpecialCaseType.PlayCardForFreeOncePerAge:
                        if (e.Info == null || (e.Info is Age && ((Age)e.Info) != age))
                        {
                            result = e;
                            continue;
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        
        private void CheckResourcesToBorrow(IStructureCard card, ITurnPlayer player)
        {
            IList<ResourceType> missingResources = new List<ResourceType>();

            if ((player.ChosenAction == TurnAction.BuyCard && (CanGetForFree(card) || HasDemandedCard(card, player.Cards))) || ValidSpecialCase(player, player.SpecialCaseToUse, age) != null)
                return;

            IList<ResourceType> resourcesToCheck = new List<ResourceType>();
            if (player.ChosenAction == TurnAction.BuildWonderStage && player.Wonder.NextStage != null)
                foreach (var c in player.Wonder.NextStage.Costs)
                    resourcesToCheck.Add(c);
            if (player.ChosenAction == TurnAction.BuyCard)
                foreach (var c in card.ResourceCosts)
                    resourcesToCheck.Add(c);
            //Remove predefined resources to borrow
            foreach (var r in player.ResourcesToBorrow)
            {
                var index = resourcesToCheck.IndexOf(r.ResourceType);
                if (index >= 0)
                    resourcesToCheck.RemoveAt(index);
            }
            missingResources = player.CheckResourceAvailability(resourcesToCheck.Where(r => r != ResourceType.Coin).ToList(), false);
            
            if(missingResources.Any())
                LoggerHelper.DebugFormat("Will need to borrow {0} resources that were not set.", missingResources.Count);
            //Add the rest of the needed resources to list
            foreach (var r in missingResources)
            {                
                player.ResourcesToBorrow.Add(new BorrowResourceData
                {
                    ChosenNeighbor = PlayerDirection.None,
                    ResourceType = r
                });
            }
        }

        #region Reward

        public void GetMultipleTimesRewards(IList<ITurnPlayer> players, IList<IStructureCard> discardedCards)
        {
            LoggerHelper.Info("Getting multiple times rewards for players.");
            foreach (var player in players)
            {
                var neighbors = NeighborsHelper.GetNeighbors(players.Select(p => (IPlayer)p).ToList(), player);
                foreach (var e in player.Wonder.EffectsAvailable)
                    GetMultipleTimesRewards(e, player, (IGamePlayer)neighbors[NeighborsHelper.RIGHTDIRECTION], (IGamePlayer)neighbors[NeighborsHelper.LEFTDIRECTION], discardedCards);
            }
        }

        public void GetRewards(IList<ITurnPlayer> players, IList<IStructureCard> discardedCards)
        {
            LoggerHelper.Info("Getting rewards for players.");
            foreach (var player in players)
            {
                var neighbors = NeighborsHelper.GetNeighbors(players.Select(p => (IPlayer)p).ToList(), player);
                if(player.ExecutedAction == TurnAction.BuyCard)
                    GetCardRewards(player, (IGamePlayer)neighbors[NeighborsHelper.RIGHTDIRECTION], (IGamePlayer)neighbors[NeighborsHelper.LEFTDIRECTION], discardedCards);
                if(player.ExecutedAction == TurnAction.BuildWonderStage)
                    GetWonderRewards(player, (IGamePlayer)neighbors[NeighborsHelper.RIGHTDIRECTION], (IGamePlayer)neighbors[NeighborsHelper.LEFTDIRECTION], discardedCards);
            }
        }

        public void GetPostGameRewards(IList<IGamePlayer> players)
        {
            LoggerHelper.Info("Getting post game rewards for players.");
            foreach (var p in players)
            {
                foreach (var e in p.Wonder.EffectsAvailable)
                {
                    switch (e.Type)
                    {
                        case EffectType.CopyGuildFromNeighbor:
                            var neighbors = NeighborsHelper.GetNeighbors(players.Select(pl => (IPlayer)pl).ToList(), p);
                            if (e.Info == null)
                                break;
                            var selectedCard = (CardName)e.Info;
                            var card = neighbors[NeighborsHelper.RIGHTDIRECTION].Cards.FirstOrDefault(c => c.Name == selectedCard);
                            if (card != null)
                                unitOfWork.AddEvent(new AddCardEvent(p, card));
                            else
                            {
                                card = neighbors[NeighborsHelper.LEFTDIRECTION].Cards.FirstOrDefault(c => c.Name == selectedCard);
                                if (card != null)
                                    unitOfWork.AddEvent(new AddCardEvent(p, card));
                            }
                            LoggerHelper.DebugFormat("Player {0} copied {1} from neighbor as part of CopyGuildFromNeighbor effect.", p.Name, selectedCard);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void GetCardRewards(ITurnPlayer player, IGamePlayer rightNeighbor, IGamePlayer leftNeighbor, IList<IStructureCard> discardedCards)
        {
            foreach (var p in player.SelectedCard.Production)
                GetOneTimeRewards(p, player, rightNeighbor, leftNeighbor, age, discardedCards);
        }

        private void GetWonderRewards(ITurnPlayer player, IGamePlayer rightNeighbor, IGamePlayer leftNeighbor, IList<IStructureCard> discardedCards)
        {
            if (player.Wonder.CurrentStage == null)
                return;
            foreach (var e in player.Wonder.CurrentStage.Effects)
                GetOneTimeRewards(e, player, rightNeighbor, leftNeighbor, age, discardedCards);
        }

        private void GetOneTimeRewards(IEffect effect, ITurnPlayer player, IGamePlayer rightNeighbor, IGamePlayer leftNeighbor, Age age, IList<IStructureCard> discardedCards)
        {
            switch (effect.Type)
            {
                case EffectType.Coin:
                    unitOfWork.AddEvent(new ReceiveCoinEvent(player, effect.Quantity));
                    LoggerHelper.DebugFormat("Player {0} get {1} coins", player.Name, effect.Quantity);
                    break;
                case EffectType.CoinPerRawMaterialCard:
                    var rawMaterialCoins = GetCoinsForStructureType(player, rightNeighbor, leftNeighbor, StructureType.RawMaterial, effect.Direction) * effect.Quantity;
                    unitOfWork.AddEvent(new ReceiveCoinEvent(player, rawMaterialCoins));
                    LoggerHelper.DebugFormat("Player {0} get {1} coins because of {2}", player.Name, rawMaterialCoins, effect.Type);
                    break;
                case EffectType.CoinPerManufacturedGoodCard:
                    var manufacturedCoins = GetCoinsForStructureType(player, rightNeighbor, leftNeighbor, StructureType.ManufacturedGood, effect.Direction) * effect.Quantity;
                    unitOfWork.AddEvent(new ReceiveCoinEvent(player, manufacturedCoins));
                    LoggerHelper.DebugFormat("Player {0} get {1} coins because of {2}", player.Name, manufacturedCoins, effect.Type);
                    break;
                case EffectType.CoinPerCommercialCard:
                    var commercialCoins = GetCoinsForStructureType(player, rightNeighbor, leftNeighbor, StructureType.Commercial, effect.Direction) * effect.Quantity;
                    unitOfWork.AddEvent(new ReceiveCoinEvent(player, commercialCoins));
                    LoggerHelper.DebugFormat("Player {0} get {1} coins because of {2}", player.Name, commercialCoins, effect.Type);
                    break;
                case EffectType.CoinPerWonderStageBuilt:
                    var stageCoins = GetCoinsForWonderStage(player, rightNeighbor, leftNeighbor, effect.Direction) * effect.Quantity;
                    unitOfWork.AddEvent(new ReceiveCoinEvent(player, stageCoins));
                    LoggerHelper.DebugFormat("Player {0} get {1} coins because of {2}", player.Name, stageCoins, effect.Type);
                    break;
                case EffectType.PlayOneDiscardedCard:
                    var discarded = (IStructureCard)player.AdditionalInfo;
                    if (!discardedCards.Contains(discarded))
                        throw new Exception("The card is not in discard pile.");
                    LoggerHelper.DebugFormat("Player {0} plays discarded card {1}", player.Name, discarded.Name);
                    unitOfWork.AddEvent(new DrawFromDiscardPileEvent(discarded, discardedCards));
                    unitOfWork.AddEvent(new AddCardEvent(player, discarded));
                    player.ExecutedAction = TurnAction.BuyCard;
                    player.SelectedCard = discarded;
                    GetCardRewards(player, rightNeighbor, leftNeighbor, discardedCards);
                    break;
                default:
                    break;
            }
        }

        private void GetMultipleTimesRewards(IEffect effect, ITurnPlayer player, IGamePlayer rightNeighbor, IGamePlayer leftNeighbor, IList<IStructureCard> discardedCards)
        {
            switch (effect.Type)
            {
                case EffectType.PlaySeventhCard:
                    if (turn != 6)
                        break;
                    LoggerHelper.DebugFormat("Player {0} plays extra card", player.Name);
                    player.SelectedCard = player.SelectableCards[0];
                    Play(player, rightNeighbor, leftNeighbor, discardedCards);
                    GetCardRewards(player, rightNeighbor, leftNeighbor, discardedCards);
                    break;
                default:
                    break;
            }
        }        

        private int GetCoinsForStructureType(IGamePlayer player, IGamePlayer rightNeighbor, IGamePlayer leftNeighbor, StructureType type, PlayerDirection direction)
        {
            var coins = 0;
            if (direction.HasFlag(PlayerDirection.Myself))
                coins += CountCards(player, type);
            if (direction.HasFlag(PlayerDirection.ToTheLeft))
                coins += CountCards(leftNeighbor, type);
            if (direction.HasFlag(PlayerDirection.ToTheRight))
                coins += CountCards(rightNeighbor, type);            
            return coins;
        }

        private int CountCards(IGamePlayer player, StructureType type)
        {
            return player.Cards.Count(c => c.Type == type);
        }

        private int GetCoinsForWonderStage(IGamePlayer player, IGamePlayer rightNeighbor, IGamePlayer leftNeighbor, PlayerDirection direction)
        {
            var coins = 0;
            if (direction.HasFlag(PlayerDirection.Myself))
                coins += player.Wonder.StagesBuilt;
            if (direction.HasFlag(PlayerDirection.ToTheLeft))
                coins += leftNeighbor.Wonder.StagesBuilt; ;
            if (direction.HasFlag(PlayerDirection.ToTheRight))
                coins += rightNeighbor.Wonder.StagesBuilt;
            return coins;
        }

        public void InitializeTurnPlayers(IList<ITurnPlayer> players)
        {
            LoggerHelper.Info("Clear player data for a new turn.");
            foreach (var p in players)
                p.InitializeTurnData();
        }

        #endregion
    }
}
