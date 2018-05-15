using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Entities
{
    public class RawMaterialCard : BaseStructureCard
    {
        public RawMaterialCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<IEffect> effects)
            : base(StructureType.RawMaterial, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }
    }
}
