using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonder.Entities
{
    public abstract class BaseStructureCard : IStructureCard
    {
        public BaseStructureCard(StructureType type, CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<IEffect> effects)
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
                this.production = new List<IEffect>();
            else
                this.production = effects;
        }

        protected StructureType type;
        protected CardName name;
        protected int playersCount;
        protected Age age;
        protected IList<ResourceType> resourceCosts;
        protected IList<CardName> cardCosts;
        protected IList<IEffect> production;

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

        public IList<IEffect> Production
        {
            get { return production; }
        }

        public IList<IEffect> StandaloneEffect
        {
            get
            {
                if (production.Count == 1)
                    return production;
                return CheckSpecialCondition(true);
            }
        }

        public IList<IEffect> ChoosableEffect
        {
            get
            {
                if (production.Count > 1)
                    return CheckSpecialCondition(false);
                return new List<IEffect>();
            }
        }

        /// <summary>
        /// When it has multiple effects, check whether they are choosable or not
        /// </summary>
        /// <param name="lookForStandalone"></param>
        /// <returns></returns>
        protected virtual IList<IEffect> CheckSpecialCondition(bool lookForStandalone)
        {
            if (lookForStandalone)
                return new List<IEffect>();
            else
                return production;
        }
    }
}
