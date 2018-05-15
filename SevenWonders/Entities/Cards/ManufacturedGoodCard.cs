using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Entities
{
    public class ManufacturedGoodCard : BaseStructureCard
    {
        public ManufacturedGoodCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<IEffect> effects)
            : base(StructureType.ManufacturedGood, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }
    }
}
