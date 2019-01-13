using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Entities
{
    public class CommercialCard : StructureCard
    {
        public CommercialCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<Effect> effects)
            : base(StructureType.Commercial, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }

        protected override IList<Effect> CheckSpecialCondition(bool lookForStandalone)
        {
            if (!Production.Any(e => Enumerator.ContainsEnumeratorValue<VictoryPointType>((int)e.Type)))
                return base.CheckSpecialCondition(lookForStandalone);
            if (lookForStandalone)
                return Production;
            else
                return new List<Effect>();
        }
    }
}
