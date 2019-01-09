using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Entities
{
    public abstract class GamePlayer : Player
    {
        public GamePlayer(string name) : base(name) { }

        #region Setup
        /// <summary>
        /// Define wonder to be played
        /// </summary>
        /// <param name="wonder"></param>
        public void SetWonder(BaseWonder wonder)
        {
            Wonder = wonder;
        }

        #endregion

        #region Resources and Cards
        /// <summary>
        /// Get all the resources available for this player.
        /// </summary>
        /// <param name="shareableOnly">Whether to select only resources that can be shared with other players or not.</param>
        /// <returns></returns>
        public virtual IList<ResourceType> GetResourcesAvailable(bool shareableOnly)
        {
            var resourcesAvailable = new List<ResourceType>();
            if (Wonder == null)
                return new List<ResourceType>();

            resourcesAvailable.Add(Wonder.OriginalResource);

            var cardsAvailable = Cards
                .Where(c => c.Type == StructureType.RawMaterial
                    || c.Type == StructureType.ManufacturedGood
                    || (shareableOnly ? false : c.Type == StructureType.Commercial)).ToList();
            foreach (var c in cardsAvailable)
            {
                var effects = c.StandaloneEffect
                    .Where(e => Helper.Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)e.Type)
                        || Helper.Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)e.Type));

                foreach (var e in effects)
                {
                    for (int i = 0; i < e.Quantity; ++i)
                        resourcesAvailable.Add((ResourceType)((int)e.Type));
                }
            }
            return resourcesAvailable;
            //As of now there is no need to check standalone resources for wonder effects as it never happens            
        }

        /// <summary>
        /// Get all the effects, except resources.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Effect> GetNonResourceEffects()
        {
            var effects = Wonder.EffectsAvailable.ToList();
            foreach (var c in Cards)
            {
                effects.AddRange(c.Production.Where(e => !Helper.Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)e.Type) && !Helper.Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)e.Type)));
            }
            return effects;
        }

        /// <summary>
        /// Check the availability for a list of resources. Return the resources that are not available.
        /// </summary>
        /// <param name="neededResources">Resources the player is in need of.</param>
        /// <param name="shareableOnly">Whether to select only resources that can be shared with other players or not.</param>
        /// <returns></returns>
        public virtual IList<ResourceType> CheckResourceAvailability(IList<ResourceType> neededResources, bool shareableOnly)
        {
            var resourcesNotAvailable = new List<ResourceType>();
            var standaloneResources = this.GetResourcesAvailable(shareableOnly);

            foreach (var res in neededResources)
            {
                int index = standaloneResources.IndexOf(res);
                if (index == -1)//not found
                    resourcesNotAvailable.Add(res);
                else
                    standaloneResources.RemoveAt(index);
            }

            return ChoosableResourceAvailability(resourcesNotAvailable, shareableOnly);
        }

        protected virtual IList<ResourceType> ChoosableResourceAvailability(IList<ResourceType> resources, bool shareableOnly)
        {
            var resourcesNotAvailable = new List<ResourceType>();
            if (!resources.Any())
                return resourcesNotAvailable;

            var choosableResources = GetChoosableResources(shareableOnly);
            var usableResources = new List<IEnumerable<ResourceType>>();
            var dicResourceAvailability = new Dictionary<ResourceType, int>();

            //For each choosable list, retrieves only the ones that can be used.
            foreach (var res in choosableResources)
            {
                var listToAdd = res.Where(r => resources.Contains(r));
                if (listToAdd.Any())
                    usableResources.Add(listToAdd);
            }
            //For each different resource counts how many times they can be used based on the choosable list.
            foreach (var res in resources.Distinct())
            {
                var available = usableResources.Count(r => r.Contains(res));
                var needed = resources.Count(r => r == res);
                if (needed < available)
                    available = needed;
                else if (available < needed)
                {
                    //Add any extra unvailable resource to the result list
                    for (int i = 0; i < needed - available; ++i)
                        resourcesNotAvailable.Add(res);
                }
                dicResourceAvailability.Add(res, available);
            }

            //Get the resources
            while (dicResourceAvailability.Any(kv => kv.Value > 0) && usableResources.Any(l => l.Any()))
            {
                usableResources = usableResources.OrderBy(r => r.Count()).ToList();
                var useResource = usableResources[0];
                var chosenResource = useResource.First();
                foreach (var r in useResource)
                {
                    int needed = dicResourceAvailability[r];
                    //Select this only if it needs more resources than the current chosen one.
                    if (needed > dicResourceAvailability[chosenResource])
                        chosenResource = r;
                }
                //Subtract one of the chosen resource count.
                dicResourceAvailability[chosenResource]--;
                //Remove the current item in the choosable list as the choice was made already.
                usableResources.RemoveAt(0);
                //If it has got all it needs, clear the rest of the entries
                if (dicResourceAvailability[chosenResource] == 0)
                {
                    for (int i = 0; i < usableResources.Count; ++i)
                    {
                        var ur = usableResources[i];
                        ur = ur.Where(r => r != chosenResource);
                    }
                }
            }
            //Add to unavailable what is left in the dictionary
            foreach (var entry in dicResourceAvailability)
            {
                for (int i = 0; i < entry.Value; ++i)
                    resourcesNotAvailable.Add(entry.Key);
            }
            return resourcesNotAvailable;
        }

        /// <summary>
        /// Get a list of choosable resources.
        /// </summary>
        /// <param name="shareableOnly">Whether to select only resources that can be shared with other players or not.</param>
        /// <returns></returns>
        public IList<IList<ResourceType>> GetChoosableResources(bool shareableOnly)
        {
            var choosableResources = new List<IList<ResourceType>>();
            var cardsAvailable = Cards
                            .Where(c => c.Type == StructureType.RawMaterial
                                || c.Type == StructureType.ManufacturedGood
                                || (shareableOnly ? false : c.Type == StructureType.Commercial)).ToList();
            foreach (var c in cardsAvailable)
            {
                var effects = c.ChoosableEffect
                    .Where(e => Helper.Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)e.Type)
                        || Helper.Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)e.Type));

                if (!effects.Any())
                    continue;

                var effectResources = new List<ResourceType>();
                foreach (var e in effects)
                {
                    for (int i = 0; i < e.Quantity; ++i)
                        effectResources.Add((ResourceType)((int)e.Type));
                }
                choosableResources.Add(effectResources);
            }

            if (!shareableOnly)
            {
                foreach (var wr in Wonder.ChoosableEffectsAvailable)
                {
                    var effects = wr.Where(e => Helper.Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)e.Type)
                        || Helper.Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)e.Type));

                    if (!effects.Any())
                        continue;

                    var effectResources = new List<ResourceType>();
                    foreach (var e in effects)
                    {
                        for (int i = 0; i < e.Quantity; ++i)
                            effectResources.Add((ResourceType)((int)e.Type));
                    }
                    choosableResources.Add(effectResources);
                }
            }

            return choosableResources;
        }

        #endregion

        #region Trade
        /// <summary>
        /// Check if the player can buy with discount from neighbor.
        /// </summary>
        /// <param name="direction">Right of left neighbor.</param>
        /// <param name="type">Type of the discount.</param>
        /// <returns></returns>
        public bool HasDiscount(PlayerDirection direction, TradeDiscountType type)
        {
            var availableDiscounts = new List<Effect>();
            foreach (var l in Cards.Where(c => c.Type == StructureType.Commercial).Select(c => c.Production))
            {
                availableDiscounts.AddRange(l.Where(e => Helper.Enumerator.ContainsEnumeratorValue<TradeDiscountType>((int)e.Type)));
            }
            availableDiscounts.AddRange(Wonder.EffectsAvailable.Where(e => Helper.Enumerator.ContainsEnumeratorValue<TradeDiscountType>((int)e.Type)));
            return availableDiscounts.Any(e => (int)e.Type == (int)type && e.Direction.HasFlag(direction));
        }

        /// <summary>
        /// Add coins to its treasury
        /// </summary>
        /// <param name="quantity">number of coins</param>
        public void ReceiveCoin(int quantity)
        {
            Coins += quantity;
        }

        /// <summary>
        /// Pay coins from its trasury
        /// </summary>
        /// <param name="quantity">number of coins</param>
        public void PayCoin(int quantity)
        {
            if (Coins - quantity < 0)
                throw new NotEnoughException("Player doesn't have enough coin to pay");
            Coins -= quantity;
        }

        #endregion

        #region Conflicts
        /// <summary>
        /// The player's current military power.
        /// </summary>
        /// <returns></returns>
        public int GetMilitaryPower()
        {
            int total = 0;

            //Try with non resource effects if at some point not only military cards provide shield.
            var militaryCards = Cards
                .Where(c => c.Type == StructureType.Military).ToList();

            foreach (var c in militaryCards)
            {
                foreach (var p in c.Production)
                {
                    if (p.Type == EffectType.Shield)
                        total += p.Quantity;
                }
            }

            total += Wonder.EffectsAvailable.Where(c => c.Type == EffectType.Shield).Sum(c => c.Quantity);

            return total;
        }

        public void AddConflictToken(ConflictToken token)
        {
            ConflictTokens.Add(token);
        }

        #endregion
    }
}
