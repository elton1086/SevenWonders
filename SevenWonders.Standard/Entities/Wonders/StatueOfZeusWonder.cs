using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public class StatueOfZeusWonder : BaseWonder
    {
        public StatueOfZeusWonder(WonderBoardSide boardSide)
            : base(boardSide)
        {
        }

        public override WonderName Name
        {
            get { return WonderName.StatueOfZeusInOlimpia; }
        }

        public override ResourceType OriginalResource
        {
            get { return ResourceType.Wood; }
        }

        protected override IList<WonderStage> CreateASideStages()
        {
            return new List<WonderStage>
            {
                new WonderStage(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 3) }),
                new WonderStage(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                    new List<Effect> { new Effect(EffectType.PlayCardForFreeOncePerAge) }),
                new WonderStage(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 7) })
            };
        }

        protected override IList<WonderStage> CreateBSideStages()
        {
            return new List<WonderStage>
            {
                new WonderStage(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                    new List<Effect> { new Effect(EffectType.BuyRawMaterialDiscount, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) }),
                new WonderStage(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 5) }),
                new WonderStage(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Loom },
                    new List<Effect> { new Effect(EffectType.CopyGuildFromNeighbor, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) })
            };
        }
    }
}
