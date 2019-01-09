using SevenWonders.BaseEntities;
using SevenWonders.Entities;
using System.Collections.Generic;

namespace SevenWonders.Contracts
{
    public interface IWonderStage
    {
        IList<ResourceType> Costs { get; }
        IList<Effect> Effects { get; }
        void AddCostsAndEffects(IList<ResourceType> costs, IList<Effect> effects);
    }
}
