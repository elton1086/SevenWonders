using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public class WonderStage : IWonderStage
    {
        private List<ResourceType> costs = new List<ResourceType>();
        private List<Effect> effects = new List<Effect>();

        public IList<ResourceType> Costs
        {
            get
            {
                return costs;
            }
        }

        public IList<Effect> Effects
        {
            get
            {
                return effects;
            }
        }


        public void AddCostsAndEffects(IList<ResourceType> costs, IList<Effect> effects)
        {
            this.costs.AddRange(costs);
            this.effects.AddRange(effects);
        }
    }
}
