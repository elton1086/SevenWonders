using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Entities
{
    public class ScientificCard : BaseStructureCard
    {
        public ScientificCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<IEffect> effects)
            : base(StructureType.Scientific, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }
    }
}
