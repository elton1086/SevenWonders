using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public class ManufacturedGoodCard : StructureCard
    {
        public ManufacturedGoodCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<Effect> effects)
            : base(StructureType.ManufacturedGood, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }
    }
}
