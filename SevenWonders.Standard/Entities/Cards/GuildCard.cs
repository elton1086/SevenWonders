﻿using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Helper;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Entities
{
    public class GuildCard : StructureCard
    {
        public GuildCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<Effect> effects)
            : base(StructureType.Guilds, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }

        protected override IList<Effect> CheckSpecialCondition(bool lookForStandalone)
        {
            if (!production.Any(e => Enumerator.ContainsEnumeratorValue<VictoryPointType>((int)e.Type)))
                return base.CheckSpecialCondition(lookForStandalone);
            if (lookForStandalone)
                return production;
            else
                return new List<Effect>();
        }
    }
}
