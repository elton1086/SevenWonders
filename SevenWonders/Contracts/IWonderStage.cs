using SevenWonder.BaseEntities;
using System.Collections.Generic;

namespace SevenWonder.Contracts
{
    public interface IWonderStage
    {
        IList<ResourceType> Costs { get; }
        IList<IEffect> Effects { get; }
        void AddCostsAndEffects(IList<ResourceType> costs, IList<IEffect> effects);
    }
}
