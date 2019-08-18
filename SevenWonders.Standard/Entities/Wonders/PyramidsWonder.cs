using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public class PyramidsWonder : BaseWonder
    {
        public PyramidsWonder(WonderBoardSide boardSide)
            : base(boardSide)
        {
        }

        public override WonderName Name
        {
            get { return WonderName.PyramidsOfGiza; }
        }

        public override ResourceType OriginalResource
        {
            get { return ResourceType.Stone; }
        }

        protected override IList<WonderStage> CreateASideStages()
        {
            return new List<WonderStage>
            {
                new WonderStage(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 3) }),
                new WonderStage(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Wood },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 5) }),
                new WonderStage(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Stone },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 7) })
            };
        }

        protected override IList<WonderStage> CreateBSideStages()
        {
            return new List<WonderStage>
            {
                new WonderStage(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 3) }),
                new WonderStage(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 5) }),
                new WonderStage(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Clay },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 5) }),
                new WonderStage(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Papyrus },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 7) })
            };
        }
    }
}
