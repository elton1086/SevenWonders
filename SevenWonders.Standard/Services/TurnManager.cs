using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Entities.Events;
using SevenWonders.Helper;
using SevenWonders.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Services
{
    public class TurnManager : ITurnManager
    {
        private readonly ITradeManager tradeManager;
        private readonly ICoreLogger logger;

        IUnitOfWork unitOfWork;

        public TurnManager(ITradeManager tradeManager, ICoreLogger logger)
        {
            this.tradeManager = tradeManager;
            this.logger = logger;
        }

        public IEnumerable<TurnPlayer> InitializeTurn(IEnumerable<GamePlayer> gamePlayers)
        {
            foreach (var p in gamePlayers)
                yield return new TurnPlayer(p);
        }

        public void SetScope(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            tradeManager.SetScope(unitOfWork);
        }

        public void Play(IList<TurnPlayer> players, IList<StructureCard> discardedCards, Age age)
        {
            logger.Info("Starting to play turn for all players");
            foreach (var p in players)
            {
                var neighbors = p.Player.GetNeighbors(players.Select(pl => pl.Player).ToList());
                Play(p, (GamePlayer)neighbors[NeighborsHelper.RIGHTDIRECTION], (GamePlayer)neighbors[NeighborsHelper.LEFTDIRECTION], discardedCards, age);
            }
        }

        private void Play(TurnPlayer player, GamePlayer rightPlayer, GamePlayer leftPlayer, IList<StructureCard> discardedCards, Age age)
        {
            logger.Debug("Player {0} will {1} {2}", player.Player.Name, player.ChosenAction, player.SelectedCard.Name);
            var success = false;
            CheckResourcesToBorrow(player.SelectedCard, player, age);
            var canPlayCard = player.CanPlayCard;
            if (!canPlayCard)
                logger.Debug("Player has the card already.");
            if (canPlayCard
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
                logger.Debug("Player {0} could not {1}. Selling and discarding card", player.Player.Name, player.ChosenAction);
                SellCard(player, discardedCards);
                player.ExecutedAction = TurnAction.SellCard;
            }
            else
            {
                player.ExecutedAction = player.ChosenAction;
            }
            RemoveCardFromSelectable(player);
        }

        private void RemoveCardFromSelectable(TurnPlayer player)
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
        private bool BuyCard(StructureCard card, TurnPlayer player, Age age, SpecialCaseType specialCase = SpecialCaseType.None)
        {
            bool result = TryBuyCard(card, player, age, specialCase);
            if (result)
            {
                logger.Debug("Card can be bought");
                unitOfWork.AddEvent(new AddCardEvent(player.Player, card));
            }
            return result;
        }

        private bool TryBuyCard(StructureCard card, TurnPlayer player, Age age, SpecialCaseType specialCase)
        {
            if (CanGetForFree(card))
            {
                logger.Debug("Card is free.");
                return true;
            }
            if (HasDemandedCard(card, player.Player.Cards))
            {
                logger.Debug("Player can build for free because of previous card");
                return true;
            }
            if (UseSpecialCase(player.Player, specialCase, age))
            {
                logger.Debug("Player can play card for free, because of {0}", specialCase);
                return true;
            }

            var coins = card.ResourceCosts.Count(r => r == ResourceType.Coin);
            var resources = card.ResourceCosts.Where(r => r != ResourceType.Coin);
            logger.Debug("Structure card costs {0} coins and {1} resources", coins, resources.Count());
            if (player.CoinsLeft >= coins && !player.Player.CheckResourceAvailability(resources.ToList(), false).Any())
            {
                if (coins > 0)
                    unitOfWork.AddEvent(new PayCoinEvent(player.Player, coins));
                return true;
            }

            return false;
        }

        private bool CanGetForFree(StructureCard card)
        {
            //The card doesn't cost anything.
            if (!card.CardCosts.Any() && !card.ResourceCosts.Any())
                return true;
            return false;
        }

        private bool HasDemandedCard(StructureCard card, IList<StructureCard> currentCards)
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

        private bool BuildWonderStage(TurnPlayer player)
        {
            bool result = TryBuildStage(player);
            if (result)
            {
                logger.Debug("Wonder stage can be built");
                unitOfWork.AddEvent(new BuildStageEvent(player.Player));
            }
            return result;
        }

        private bool TryBuildStage(TurnPlayer player)
        {
            if (player.Player.Wonder.NextStage == null)
                return false;
            var costs = player.Player.Wonder.NextStage.Costs;
            var coins = costs.Count(r => r == ResourceType.Coin);
            var resources = costs.Where(r => r != ResourceType.Coin);
            if (player.CoinsLeft >= coins && !player.Player.CheckResourceAvailability(resources.ToList(), false).Any())
            {
                if (coins > 0)
                    unitOfWork.AddEvent(new PayCoinEvent(player.Player, coins));
                return true;
            }
            return false;
        }

        #endregion

        private void SellCard(TurnPlayer player, IList<StructureCard> discardedCards)
        {
            logger.Debug("Discard {0} and collect coins", player.SelectedCard.Name);
            unitOfWork.AddEvent(new DiscardCardEvent(player.SelectedCard, discardedCards));
            unitOfWork.AddEvent(new ReceiveCoinEvent(player.Player, ConstantValues.SELL_CARD_COINS));
        }

        private bool UseSpecialCase(GamePlayer player, SpecialCaseType specialCase, Age age)
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

        private Effect ValidSpecialCase(GamePlayer player, SpecialCaseType specialCase, Age age)
        {
            Effect result = null;
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

        private void CheckResourcesToBorrow(StructureCard card, TurnPlayer player, Age age)
        {
            IList<ResourceType> missingResources = new List<ResourceType>();

            if ((player.ChosenAction == TurnAction.BuyCard && (CanGetForFree(card) || HasDemandedCard(card, player.Player.Cards))) || ValidSpecialCase(player.Player, player.SpecialCaseToUse, age) != null)
                return;

            IList<ResourceType> resourcesToCheck = new List<ResourceType>();
            if (player.ChosenAction == TurnAction.BuildWonderStage && player.Player.Wonder.NextStage != null)
                foreach (var c in player.Player.Wonder.NextStage.Costs)
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
            missingResources = player.Player.CheckResourceAvailability(resourcesToCheck.Where(r => r != ResourceType.Coin).ToList(), false);

            if (missingResources.Any())
                logger.Debug("Will need to borrow {0} resources that were not set.", missingResources.Count);
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

        public void GetMultipleTimesRewards(IList<TurnPlayer> players, IList<StructureCard> discardedCards, int turn, Age age)
        {
            logger.Info("Getting multiple times rewards for players.");
            foreach (var p in players)
            {
                var neighbors = p.Player.GetNeighbors(players.Select(pl => pl.Player).ToList());
                foreach (var e in p.Player.Wonder.EffectsAvailable)
                    GetMultipleTimesRewards(e, p, (GamePlayer)neighbors[NeighborsHelper.RIGHTDIRECTION], (GamePlayer)neighbors[NeighborsHelper.LEFTDIRECTION], discardedCards, turn, age);
            }
        }

        public void GetRewards(IList<TurnPlayer> players, IList<StructureCard> discardedCards, Age age)
        {
            logger.Info("Getting rewards for players.");
            foreach (var p in players)
            {
                var neighbors = p.Player.GetNeighbors(players.Select(pl => pl.Player).ToList());
                if (p.ExecutedAction == TurnAction.BuyCard)
                    GetCardRewards(p, (GamePlayer)neighbors[NeighborsHelper.RIGHTDIRECTION], (GamePlayer)neighbors[NeighborsHelper.LEFTDIRECTION], discardedCards, age);
                if (p.ExecutedAction == TurnAction.BuildWonderStage)
                    GetWonderRewards(p, (GamePlayer)neighbors[NeighborsHelper.RIGHTDIRECTION], (GamePlayer)neighbors[NeighborsHelper.LEFTDIRECTION], discardedCards, age);
            }
        }

        public void GetPostGameRewards(IList<GamePlayer> players)
        {
            logger.Info("Getting post game rewards for players.");
            foreach (var p in players)
            {
                foreach (var e in p.Wonder.EffectsAvailable)
                {
                    switch (e.Type)
                    {
                        case EffectType.CopyGuildFromNeighbor:
                            var neighbors = p.GetNeighbors(players);
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
                            logger.Debug("Player {0} copied {1} from neighbor as part of CopyGuildFromNeighbor effect.", p.Name, selectedCard);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void GetCardRewards(TurnPlayer player, GamePlayer rightNeighbor, GamePlayer leftNeighbor, IList<StructureCard> discardedCards, Age age)
        {
            foreach (var p in player.SelectedCard.Production)
                GetOneTimeRewards(p, player, rightNeighbor, leftNeighbor, age, discardedCards);
        }

        private void GetWonderRewards(TurnPlayer player, GamePlayer rightNeighbor, GamePlayer leftNeighbor, IList<StructureCard> discardedCards, Age age)
        {
            if (player.Player.Wonder.CurrentStage == null)
                return;
            foreach (var e in player.Player.Wonder.CurrentStage.Effects)
                GetOneTimeRewards(e, player, rightNeighbor, leftNeighbor, age, discardedCards);
        }

        private void GetOneTimeRewards(Effect effect, TurnPlayer player, GamePlayer rightNeighbor, GamePlayer leftNeighbor, Age age, IList<StructureCard> discardedCards)
        {
            switch (effect.Type)
            {
                case EffectType.Coin:
                    unitOfWork.AddEvent(new ReceiveCoinEvent(player.Player, effect.Quantity));
                    logger.Debug("Player {0} get {1} coins", player.Player.Name, effect.Quantity);
                    break;
                case EffectType.CoinPerRawMaterialCard:
                    var rawMaterialCoins = GetCoinsForStructureType(player.Player, rightNeighbor, leftNeighbor, StructureType.RawMaterial, effect.Direction) * effect.Quantity;
                    unitOfWork.AddEvent(new ReceiveCoinEvent(player.Player, rawMaterialCoins));
                    logger.Debug("Player {0} get {1} coins because of {2}", player.Player.Name, rawMaterialCoins, effect.Type);
                    break;
                case EffectType.CoinPerManufacturedGoodCard:
                    var manufacturedCoins = GetCoinsForStructureType(player.Player, rightNeighbor, leftNeighbor, StructureType.ManufacturedGood, effect.Direction) * effect.Quantity;
                    unitOfWork.AddEvent(new ReceiveCoinEvent(player.Player, manufacturedCoins));
                    logger.Debug("Player {0} get {1} coins because of {2}", player.Player.Name, manufacturedCoins, effect.Type);
                    break;
                case EffectType.CoinPerCommercialCard:
                    var commercialCoins = GetCoinsForStructureType(player.Player, rightNeighbor, leftNeighbor, StructureType.Commercial, effect.Direction) * effect.Quantity;
                    unitOfWork.AddEvent(new ReceiveCoinEvent(player.Player, commercialCoins));
                    logger.Debug("Player {0} get {1} coins because of {2}", player.Player.Name, commercialCoins, effect.Type);
                    break;
                case EffectType.CoinPerWonderStageBuilt:
                    var stageCoins = GetCoinsForWonderStage(player.Player, rightNeighbor, leftNeighbor, effect.Direction) * effect.Quantity;
                    unitOfWork.AddEvent(new ReceiveCoinEvent(player.Player, stageCoins));
                    logger.Debug("Player {0} get {1} coins because of {2}", player.Player.Name, stageCoins, effect.Type);
                    break;
                case EffectType.PlayOneDiscardedCard:
                    var discarded = (StructureCard)player.AdditionalInfo;
                    if (!discardedCards.Contains(discarded))
                        throw new Exception("The card is not in discard pile.");
                    logger.Debug("Player {0} plays discarded card {1}", player.Player.Name, discarded.Name);
                    unitOfWork.AddEvent(new DrawFromDiscardPileEvent(discarded, discardedCards));
                    unitOfWork.AddEvent(new AddCardEvent(player.Player, discarded));
                    player.ExecutedAction = TurnAction.BuyCard;
                    player.SelectedCard = discarded;
                    GetCardRewards(player, rightNeighbor, leftNeighbor, discardedCards, age);
                    break;
                default:
                    break;
            }
        }

        private void GetMultipleTimesRewards(Effect effect, TurnPlayer player, GamePlayer rightNeighbor, GamePlayer leftNeighbor, IList<StructureCard> discardedCards, int turn, Age age)
        {
            switch (effect.Type)
            {
                case EffectType.PlaySeventhCard:
                    if (turn != 6)
                        break;
                    logger.Debug("Player {0} plays extra card", player.Player.Name);
                    player.SelectedCard = player.SelectableCards[0];
                    Play(player, rightNeighbor, leftNeighbor, discardedCards, age);
                    GetCardRewards(player, rightNeighbor, leftNeighbor, discardedCards, age);
                    break;
                default:
                    break;
            }
        }

        private int GetCoinsForStructureType(GamePlayer player, GamePlayer rightNeighbor, GamePlayer leftNeighbor, StructureType type, PlayerDirection direction)
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

        private int CountCards(GamePlayer player, StructureType type)
        {
            return player.Cards.Count(c => c.Type == type);
        }

        private int GetCoinsForWonderStage(GamePlayer player, GamePlayer rightNeighbor, GamePlayer leftNeighbor, PlayerDirection direction)
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

        #endregion
    }
}
