using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public class ScientificCard : StructureCard
    {
        public ScientificCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<Effect> effects)
            : base(StructureType.Scientific, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }
    }
}
