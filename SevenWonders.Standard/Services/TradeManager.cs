using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Entities.Events;
using SevenWonders.Helper;
using SevenWonders.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Services
{
    public class TradeManager : ITradeManager
    {
        private IUnitOfWork unitOfWork;
        private readonly ICoreLogger logger;

        public TradeManager(ICoreLogger logger)
        {            
            this.logger = logger;
        }

        public void SetScope(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void SetupCoinsFromBank(IList<GamePlayer> players)
        {
            if (players == null)
                return;
            logger.Debug("Giving {0} coins to each player", ConstantValues.INITIAL_COINS);
            foreach (var p in players)
            {
                p.ReceiveCoin(ConstantValues.INITIAL_COINS);
            }
        }

        public bool BorrowResources(TurnPlayer player, GamePlayer rightNeighbor, GamePlayer leftNeighbor, IList<BorrowResourceData> data, bool allowAutomaticChoice)
        {
            if (!data.Any())
                return true;

            //If some of the data doesn't have a direction preference and is not allowed to automatic choose, nothing happens
            if (!allowAutomaticChoice && data.Any(r => r.ChosenNeighbor != PlayerDirection.ToTheLeft && r.ChosenNeighbor != PlayerDirection.ToTheRight))
                return false;

            //Select a random direction for the non preference entries.
            foreach (var r in data.Where(r => r.ChosenNeighbor != PlayerDirection.ToTheLeft && r.ChosenNeighbor != PlayerDirection.ToTheRight))
            {
                var availableDirections = new List<PlayerDirection>();
                availableDirections.Add(PlayerDirection.ToTheLeft);
                availableDirections.Add(PlayerDirection.ToTheRight);
                r.ChosenNeighbor = Randomizer.SelectOne(availableDirections);
            }

            var fromRight = data.Where(r => r.ChosenNeighbor == PlayerDirection.ToTheRight).Select(r => r.ResourceType).ToList();
            var fromLeft = data.Where(r => r.ChosenNeighbor == PlayerDirection.ToTheLeft).Select(r => r.ResourceType).ToList();
            logger.Debug("Player {0} will borrow {1} resources from right neighbor and {2} resources from left neighbor.", player.GamePlayer.Name, fromRight.Count, fromLeft.Count);

            var missingFromRight = BorrowResourceFromNeighbor(rightNeighbor, fromRight);
            var missingFromLeft = BorrowResourceFromNeighbor(leftNeighbor, fromLeft);

            if (missingFromRight.Any() || missingFromLeft.Any())
            {                
                //If not allowed to automatic choose and some resource is missing, it can't be completed.
                if (!allowAutomaticChoice)
                    return false;

                logger.Debug("Player {0} could not borrow {1} resources from right neighbor and {2} resources from left neighbor.", player.GamePlayer.Name, fromRight.Count, fromLeft.Count);
                //Move missing resources between neighbors and try to borrow again.
                RemoveResourcesAndAddRange(fromRight, missingFromRight, missingFromLeft);
                RemoveResourcesAndAddRange(fromLeft, missingFromLeft, missingFromRight);

                if (BorrowResourceFromNeighbor(rightNeighbor, fromRight).Any() || BorrowResourceFromNeighbor(leftNeighbor, fromLeft).Any())
                    return false;

                //Clears former data and create new using the correct neighbors.
                data.Clear();
                foreach (var rt in fromRight)
                    data.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheRight, ResourceType = rt });
                foreach(var rt in fromLeft)
                    data.Add(new BorrowResourceData { ChosenNeighbor = PlayerDirection.ToTheLeft, ResourceType = rt });
            }

            //Checks if got enough coins to pay.
            foreach (var d in data)
            {
                player.CoinsLeft -= player.GamePlayer.HasDiscount(d.ChosenNeighbor, Enumerator.GetTradeDiscountType(d.ResourceType)) ? ConstantValues.COIN_VALUE_FOR_SHARE_DISCOUNT : ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT;
                if (player.CoinsLeft < 0)
                {
                    logger.Debug("Player {0} cannot afford costs in coins. Will not be able to borrow resources.", player.GamePlayer.Name);
                    return false;
                }
            }

            //If everything went ok
            player.AddTemporaryResources(data.Select(r => r.ResourceType).ToList());
            SaveEventOperations(player.GamePlayer, rightNeighbor, leftNeighbor, data);
            return true;
        }

        private static void RemoveResourcesAndAddRange(List<ResourceType> resourceList, IList<ResourceType> missingResources, IList<ResourceType> newResources)
        {
            foreach (var rt in missingResources)
            {
                if (!resourceList.Contains(rt))
                    continue;
                resourceList.RemoveAt(resourceList.IndexOf(rt));
            }
            resourceList.AddRange(newResources);
        }

        private IList<ResourceType> BorrowResourceFromNeighbor(GamePlayer neighbor, IList<ResourceType> resources)
        {
            if (!resources.Any())
                return new List<ResourceType>();
            return neighbor.CheckResourceAvailability(resources, true);
        }

        private void SaveEventOperations(GamePlayer player, GamePlayer rightNeighbor, GamePlayer leftNeighbor, IList<BorrowResourceData> data)
        {
            foreach (var entry in data)
            {
                var discountType = Enumerator.GetTradeDiscountType(entry.ResourceType);
                var coinsToPay = player.HasDiscount(entry.ChosenNeighbor, discountType) ? ConstantValues.COIN_VALUE_FOR_SHARE_DISCOUNT : ConstantValues.COIN_VALUE_FOR_SHARE_DEFAULT;

                GamePlayer neighborPlayer = null;
                switch (entry.ChosenNeighbor)
                {                 
                    case PlayerDirection.ToTheLeft:
                        neighborPlayer = leftNeighbor;
                        break;
                    case PlayerDirection.ToTheRight:
                        neighborPlayer = rightNeighbor;
                        break;
                    default:
                        break;
                }
                if (neighborPlayer == null)
                    continue;

                logger.Debug("Player {0} will pay {1} coins to {2} for using the resource {3}", player.Name, coinsToPay, neighborPlayer.Name, entry.ResourceType);
                this.unitOfWork.AddEvent(new PayCoinEvent(player, coinsToPay));
                this.unitOfWork.AddEvent(new ReceiveCoinEvent(neighborPlayer, coinsToPay));
            }
        }

        public void ResolveMilitaryConflicts(IList<GamePlayer> players, Age age)
        {
            logger.Info("Resolving military conflicts.");
            foreach (var p in players)
            {
                var neighbors = p.GetNeighbors(players);
                var rightPlayer = neighbors[NeighborsHelper.RIGHTDIRECTION];
                var leftPlayer = neighbors[NeighborsHelper.LEFTDIRECTION];

                if (CompareMilitaryPower(p, rightPlayer))
                    CollectTokens(p, rightPlayer, age);
                if (CompareMilitaryPower(p, leftPlayer))
                    CollectTokens(p, leftPlayer, age);
            }
        }

        private bool CompareMilitaryPower(GamePlayer player, GamePlayer neighbor)
        {
            return player.GetMilitaryPower() > neighbor.GetMilitaryPower();
        }

        private void CollectTokens(GamePlayer winner, GamePlayer loser, Age age)
        {
            var token = ConflictToken.AgeOneVictory;
            switch (age)
            {
                case Age.I:
                    token = ConflictToken.AgeOneVictory; 
                    break;
                case Age.II:
                    token = ConflictToken.AgeTwoVictory;
                    break;
                case Age.III:
                    token = ConflictToken.AgeThreeVictory;
                    break;
                default:
                    break;
            }
            logger.Debug("{0} wins and receives a {1} token. {2} loses.", winner.Name, token, loser.Name);
            winner.ConflictTokens.Add(token);
            loser.ConflictTokens.Add(ConflictToken.Defeat);
        }
    }
}
