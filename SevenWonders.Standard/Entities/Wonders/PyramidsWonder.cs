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

        protected override void CreateASideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 3) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Wood },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 5) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Stone },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 7) });
        }

        protected override void CreateBSideStages()
        {
            InitializeStages(4);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 3) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 5) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Clay },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 5) });
            Stages[3].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Papyrus },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 7) });
        }
    }
}
