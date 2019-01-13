using SevenWonders.BaseEntities;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public abstract class StructureCard
    {
        public StructureCard(StructureType type, CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<Effect> effects)
        {
            Type = type;
            Name = name;
            PlayersCount = playersCount;
            Age = age;

            if (resourceCosts != null)
                ResourceCosts = resourceCosts;

            if (cardCosts != null)
                CardCosts = cardCosts;

            if (effects != null)
                Production = effects;
        }

        public StructureType Type { get; private set; }
        public CardName Name { get; private set; }
        public int PlayersCount { get; private set; }
        public Age Age { get; private set; }
        public IList<ResourceType> ResourceCosts { get; private set; } = new List<ResourceType>();
        public IList<CardName> CardCosts { get; private set; } = new List<CardName>();
        public IList<Effect> Production { get; private set; } = new List<Effect>();

        public IList<Effect> StandaloneEffect
        {
            get
            {
                if (Production.Count == 1)
                    return Production;
                return CheckSpecialCondition(true);
            }
        }

        public IList<Effect> ChoosableEffect
        {
            get
            {
                if (Production.Count > 1)
                    return CheckSpecialCondition(false);
                return new List<Effect>();
            }
        }

        /// <summary>
        /// When it has multiple effects, check whether they are choosable or not
        /// </summary>
        /// <param name="lookForStandalone"></param>
        /// <returns></returns>
        protected virtual IList<Effect> CheckSpecialCondition(bool lookForStandalone)
        {
            if (lookForStandalone)
                return new List<Effect>();
            else
                return Production;
        }
    }
}
