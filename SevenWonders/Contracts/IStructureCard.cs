using SevenWonder.BaseEntities;
using System.Collections.Generic;

namespace SevenWonder.Contracts
{
    public interface IStructureCard
    {
        StructureType Type { get; }
        CardName Name { get; }
        int PlayersCount { get; }
        Age Age { get; }
        IList<ResourceType> ResourceCosts { get; }
        IList<CardName> CardCosts { get; }
        IList<IEffect> Production { get; }

        IList<IEffect> StandaloneEffect { get; }
        IList<IEffect> ChoosableEffect { get; }
    }
}
