using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public class CivilianCard : StructureCard
    {
        public CivilianCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<Effect> effects)
            : base(StructureType.Civilian, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }
    }
}
