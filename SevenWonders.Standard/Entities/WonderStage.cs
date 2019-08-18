using SevenWonders.BaseEntities;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public class WonderStage
    {
        public IList<ResourceType> Costs { get; }
        public IList<Effect> Effects { get; }

        public WonderStage(IList<ResourceType> costs, IList<Effect> effects)
        {
            Costs = costs ?? new List<ResourceType>();
            Effects = effects ?? new List<Effect>();
        }
    }
}
