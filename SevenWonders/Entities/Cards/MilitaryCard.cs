using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Entities
{
    public class MilitaryCard : BaseStructureCard
    {
        public MilitaryCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<IEffect> effects)
            : base(StructureType.Military, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }
    }
}
