﻿using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Exceptions;
using SevenWonder.Helper;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonder.Entities
{
    public abstract class Player : IGamePlayer
    {
        private int coins;
        private IWonder wonder = null;
        private List<IStructureCard> cards = new List<IStructureCard>();        
        private string name;
        private List<ConflictToken> conflictTokens = new List<ConflictToken>();

        public Player(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public IWonder Wonder
        {
            get { return wonder; }
        }

        public IList<IStructureCard> Cards
        {
            get { return cards; }
        }

        public int VictoryPoints { get; set; }

        #region Setup

        public void SetWonder(IWonder wonder)
        {
            this.wonder = wonder;
        }

        #endregion

        #region Resources and Cards

        public virtual IList<ResourceType> GetResourcesAvailable(bool shareableOnly)
        {
            var resourcesAvailable = new List<ResourceType>();
            if (wonder == null)
                return new List<ResourceType>();

            resourcesAvailable.Add(wonder.OriginalResource);
            
            var cardsAvailable = cards
                .Where(c => c.Type == StructureType.RawMaterial
                    || c.Type == StructureType.ManufacturedGood
                    || (shareableOnly ? false : c.Type == StructureType.Commercial)).ToList();
            foreach (var c in cardsAvailable)
            {
                var effects = c.StandaloneEffect
                    .Where(e => Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)e.Type)
                        || Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)e.Type));

                foreach (var e in effects)
                {
                    for (int i = 0; i < e.Quantity; ++i)
                        resourcesAvailable.Add((ResourceType)((int)e.Type));
                }
            }
            return resourcesAvailable;
            //As of now there is no need to check standalone resources for wonder effects as it never happens            
        }

        public virtual IList<IEffect> GetNonResourceEffects()
        {
            var effects = wonder.EffectsAvailable.ToList();
            foreach (var c in cards)
            {
                effects.AddRange(c.Production.Where(e => !Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)e.Type) && !Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)e.Type)));
            }
            return effects;
        }

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
            foreach(var res in resources.Distinct())
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
                    for(int i = 0; i < usableResources.Count; ++i)
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

        public IList<IList<ResourceType>> GetChoosableResources(bool shareableOnly)
        {
            var choosableResources = new List<IList<ResourceType>>();
            var cardsAvailable = cards
                            .Where(c => c.Type == StructureType.RawMaterial
                                || c.Type == StructureType.ManufacturedGood
                                || (shareableOnly ? false : c.Type == StructureType.Commercial)).ToList();
            foreach (var c in cardsAvailable)
            {
                var effects = c.ChoosableEffect
                    .Where(e => Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)e.Type)
                        || Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)e.Type));

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
                foreach (var wr in wonder.ChoosableEffectsAvailable)
                {
                    var effects = wr.Where(e => Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)e.Type)
                        || Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)e.Type));

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

        public int CardTypeCount(StructureType type)
        {
            return cards.Count(c => c.Type == type);
        }
        
        public bool HasCard(IStructureCard card)
        {
            var result = this.Cards.Any(c => c.Name == card.Name);
            if(result)
                LoggerHelper.Debug("Player has the card already.");
            return result;
        }

        #endregion

        #region Trade

        public int Coins
        {
            get { return coins; }
        }

        public bool HasDiscount(PlayerDirection direction, TradeDiscountType type)
        {
            var availableDiscounts = new List<IEffect>();
            foreach (var l in cards.Where(c => c.Type == StructureType.Commercial).Select(c => c.Production))
            {
                availableDiscounts.AddRange(l.Where(e => Enumerator.ContainsEnumeratorValue<TradeDiscountType>((int)e.Type)));
            }
            availableDiscounts.AddRange(wonder.EffectsAvailable.Where(e => Enumerator.ContainsEnumeratorValue<TradeDiscountType>((int)e.Type)));
            return availableDiscounts.Any(e => (int)e.Type == (int)type && e.Direction.HasFlag(direction));
        }

        public void ReceiveCoin(int quantity)
        {
            coins += quantity;
        }

        public void PayCoin(int quantity)
        {
            if (coins - quantity < 0)
                throw new NotEnoughException("Player doesn't have enough coin to pay");
            coins -= quantity;
        }

        #endregion

        #region Conflicts

        public int GetMilitaryPower()
        {
            int total = 0;

            //Try with non resource effects if at some point not only military cards provide shield.
            var militaryCards = cards
                .Where(c => c.Type == StructureType.Military).ToList();

            foreach (var c in militaryCards)
            {
                foreach (var p in c.Production)
                {
                    if (p.Type == EffectType.Shield)
                        total += p.Quantity;
                }
            }

            total += this.wonder.EffectsAvailable.Where(c => c.Type == EffectType.Shield).Sum(c => c.Quantity);

            return total;
        }

        public void AddConflictToken(ConflictToken token)
        {
            conflictTokens.Add(token);
        }

        public int ConflictTokenSum
        {
            get
            {
                int result = 0;
                foreach(var t in conflictTokens)
                    result += (int)t;
                return result;
            }
        }

        public IList<ConflictToken> ConflictTokens
        {
            get
            {
                return this.conflictTokens;
            }
        }

        #endregion
    }
}
