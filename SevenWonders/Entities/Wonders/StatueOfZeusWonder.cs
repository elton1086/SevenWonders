using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;

namespace SevenWonder.Entities
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

        protected override void CreateASideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 3) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                new List<IEffect> { new Effect(EffectType.PlayCardForFreeOncePerAge) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 7) });
        }

        protected override void CreateBSideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                new List<IEffect> { new Effect(EffectType.BuyRawMaterialDiscount, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 5) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Loom },
                new List<IEffect> { new Effect(EffectType.CopyGuildFromNeighbor, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) });
        }
    }
}
