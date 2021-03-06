﻿using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Helper;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonder.Entities
{
    public class GuildCard : BaseStructureCard
    {
        public GuildCard(CardName name, int playersCount, Age age, IList<ResourceType> resourceCosts, IList<CardName> cardCosts, IList<IEffect> effects)
            : base(StructureType.Guilds, name, playersCount, age, resourceCosts, cardCosts, effects)
        {
        }

        protected override IList<IEffect> CheckSpecialCondition(bool lookForStandalone)
        {
            if (!production.Any(e => Enumerator.ContainsEnumeratorValue<VictoryPointType>((int)e.Type)))
                return base.CheckSpecialCondition(lookForStandalone);
            if (lookForStandalone)
                return production;
            else
                return new List<IEffect>();
        }
    }
}
