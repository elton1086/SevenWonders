using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public class RawMaterialCard : StructureCard
    {
        public RawMaterialCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<Effect> effects)
            : base(StructureType.RawMaterial, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }
    }
}
