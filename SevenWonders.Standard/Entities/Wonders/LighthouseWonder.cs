using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Entities
{
    public class LighthouseWonder : BaseWonder
    {
        public LighthouseWonder(WonderBoardSide boardSide)
            : base(boardSide)
        {
        }

        public override WonderName Name
        {
            get { return WonderName.LighthouseOfAlexandria; }
        }

        public override ResourceType OriginalResource
        {
            get { return ResourceType.Glass; }
        }

        protected override void CreateASideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 3) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Ore, ResourceType.Ore },
                new List<Effect> { new Effect(EffectType.Ore), new Effect(EffectType.Wood), new Effect(EffectType.Clay), new Effect(EffectType.Stone) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Glass, ResourceType.Glass },
                new List<Effect> { new Effect(EffectType.VictoryPoint, 7) });
        }

        protected override void CreateBSideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay },
                new List<Effect> { new Effect(EffectType.Ore), new Effect(EffectType.Wood), new Effect(EffectType.Clay), new Effect(EffectType.Stone) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                new List<Effect> { new Effect(EffectType.Loom), new Effect(EffectType.Glass), new Effect(EffectType.Papyrus) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone},
                new List<Effect> { new Effect(EffectType.VictoryPoint, 7) });
        }

        public override IList<IList<Effect>> ChoosableEffectsAvailable
        {
            get
            {
                var result = new List<IList<Effect>>();
                result.Add(effectsInUse.Where(e => Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)e.Type)).ToList());
                result.Add(effectsInUse.Where(e => Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)e.Type)).ToList());
                return result;
            }
        }

        public override IList<Effect> EffectsAvailable
        {
            get
            {
                return effectsInUse.Where(e => !Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)e.Type) && !Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)e.Type)).ToList();
            }
        }
    }
}
