using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public class MilitaryCard : StructureCard
    {
        public MilitaryCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<Effect> effects)
            : base(StructureType.Military, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }
    }
}
