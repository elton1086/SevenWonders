using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public class TempleOfArthemisWonder : BaseWonder
    {
        public TempleOfArthemisWonder(WonderBoardSide boardSide)
            : base(boardSide)
        {
        }

        public override WonderName Name
        {
            get { return WonderName.TempleOfArthemisInEphesus; }
        }

        public override ResourceType OriginalResource
        {
            get { return ResourceType.Papyrus; }
        }

        protected override IList<WonderStage> CreateASideStages()
        {
            return new List<WonderStage>
            {
                new WonderStage(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 3) }),
                new WonderStage(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                    new List<Effect> { new Effect(EffectType.Coin, 9) }),
                new WonderStage(new List<ResourceType> { ResourceType.Papyrus, ResourceType.Papyrus },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 7) })
            };
        }

        protected override IList<WonderStage> CreateBSideStages()
        {
            return new List<WonderStage>
            {
                new WonderStage(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 2), new Effect(EffectType.Coin, 4) }),
                new WonderStage(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 3), new Effect(EffectType.Coin, 4) }),
                new WonderStage(new List<ResourceType> { ResourceType.Papyrus, ResourceType.Loom, ResourceType.Glass },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 5), new Effect(EffectType.Coin, 4) })
            };
        }
    }
}
