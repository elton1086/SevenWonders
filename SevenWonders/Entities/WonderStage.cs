using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;

namespace SevenWonder.Entities
{
    public class WonderStage : IWonderStage
    {
        private List<ResourceType> costs = new List<ResourceType>();
        private List<IEffect> effects = new List<IEffect>();

        public IList<ResourceType> Costs
        {
            get
            {
                return costs;
            }
        }

        public IList<IEffect> Effects
        {
            get
            {
                return effects;
            }
        }


        public void AddCostsAndEffects(IList<ResourceType> costs, IList<IEffect> effects)
        {
            this.costs.AddRange(costs);
            this.effects.AddRange(effects);
        }
    }
}
