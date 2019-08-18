using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System;
using System.Collections.Generic;

namespace SevenWonders.Entities
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

        protected override IList<WonderStage> CreateASideStages()
        {
            return new List<WonderStage>
            {
                new WonderStage(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 3) }),
                new WonderStage(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Ore },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 1), new Effect(EffectType.PlayOneDiscardedCard) }),
                new WonderStage(new List<ResourceType> { ResourceType.Loom, ResourceType.Loom },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 7) })
            };
        }

        protected override IList<WonderStage> CreateBSideStages()
        {
            return new List<WonderStage>
            {
                new WonderStage(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 2), new Effect(EffectType.PlayOneDiscardedCard) }),
                new WonderStage(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Clay },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 1), new Effect(EffectType.PlayOneDiscardedCard) }),
                new WonderStage(new List<ResourceType> { ResourceType.Loom, ResourceType.Glass, ResourceType.Papyrus },
                    new List<Effect> { new Effect(EffectType.PlayOneDiscardedCard) })
            };
        }
    }
}
