using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Entities
{
    public class CivilianCard : BaseStructureCard
    {
        public CivilianCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<IEffect> effects)
            : base(StructureType.Civilian, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }
    }
}
