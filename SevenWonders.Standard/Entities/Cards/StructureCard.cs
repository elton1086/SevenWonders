using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public abstract class StructureCard
    {
        public StructureCard(StructureType type, CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<Effect> effects)
        {
            this.type = type;
            this.name = name;
            this.playersCount = playersCount;
            this.age = age;
            if (resourceCosts == null)
                this.resourceCosts = new List<ResourceType>();
            else
                this.resourceCosts = resourceCosts;

            if (cardCosts == null)
                this.cardCosts = new List<CardName>();
            else
                this.cardCosts = cardCosts;

            if (effects == null)
                this.production = new List<Effect>();
            else
                this.production = effects;
        }

        protected StructureType type;
        protected CardName name;
        protected int playersCount;
        protected Age age;
        protected IList<ResourceType> resourceCosts;
        protected IList<CardName> cardCosts;
        protected IList<Effect> production;

        public StructureType Type
        {
            get { return type; }
        }

        public CardName Name
        {
            get { return name; }
        }

        public int PlayersCount
        {
            get { return playersCount; }
        }

        public Age Age
        {
            get { return age; }
        }

        public IList<ResourceType> ResourceCosts
        {
            get { return resourceCosts; }
        }

        public IList<CardName> CardCosts
        {
            get { return cardCosts; }
        }

        public IList<Effect> Production
        {
            get { return production; }
        }

        public IList<Effect> StandaloneEffect
        {
            get
            {
                if (production.Count == 1)
                    return production;
                return CheckSpecialCondition(true);
            }
        }

        public IList<Effect> ChoosableEffect
        {
            get
            {
                if (production.Count > 1)
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
                return production;
        }
    }
}
