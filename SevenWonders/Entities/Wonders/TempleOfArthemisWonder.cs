using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;

namespace SevenWonder.Entities
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

        protected override void CreateASideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 3) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                new List<IEffect> { new Effect(EffectType.Coin, 9) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Papyrus, ResourceType.Papyrus },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 7) });
        }

        protected override void CreateBSideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 2), new Effect(EffectType.Coin, 4) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 3), new Effect(EffectType.Coin, 4) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Papyrus, ResourceType.Loom, ResourceType.Glass },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 5), new Effect(EffectType.Coin, 4) });
        }
    }
}
