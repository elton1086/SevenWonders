using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;

namespace SevenWonder.Entities
{
    public class MausoleumWonder : BaseWonder
    {
        public MausoleumWonder(WonderBoardSide boardSide)
            : base(boardSide)
        {
        }

        public override WonderName Name
        {
            get { return WonderName.MausoleumOfHalicarnassus; }
        }

        public override ResourceType OriginalResource
        {
            get { return ResourceType.Loom; }
        }

        protected override void CreateASideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 3) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Ore },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 1), new Effect(EffectType.PlayOneDiscardedCard) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Loom, ResourceType.Loom },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 7) });
        }

        protected override void CreateBSideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 2), new Effect(EffectType.PlayOneDiscardedCard) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Clay },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 1), new Effect(EffectType.PlayOneDiscardedCard) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Loom, ResourceType.Glass, ResourceType.Papyrus },
                new List<IEffect> { new Effect(EffectType.PlayOneDiscardedCard) });
        }
    }
}
