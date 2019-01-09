using SevenWonders.BaseEntities;
using System;
using System.Collections.Generic;
using SevenWonders.Helper;
using SevenWonders.Contracts;

namespace SevenWonders.Entities
{
    public class ColossusWonder : BaseWonder
    {
        public ColossusWonder(WonderBoardSide boardSide)
            : base(boardSide)
        {

        }

        public override WonderName Name
        {
            get { return WonderName.ColossusOfRhodes; }
        }

        public override ResourceType OriginalResource
        {
            get { return ResourceType.Ore; }
        }

        protected override void CreateASideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 3) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Clay },
                new List<Effect> { new Effect(EffectType.Shield, 2) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Ore, ResourceType.Ore },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 7) });
        }

        protected override void CreateBSideStages()
        {
            InitializeStages(2);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone },
                new List<Effect> { new Effect(EffectType.Shield, 1), new Effect(EffectType.Coin, 3), new Effect(EffectType.VictoryPoint, 3) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Ore, ResourceType.Ore },
                new List<Effect> { new Effect(EffectType.Shield, 1), new Effect(EffectType.Coin, 4), new Effect(EffectType.VictoryPoint, 4) });
        }
    }
}
