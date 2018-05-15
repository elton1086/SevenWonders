using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonder.Entities
{
    public class HangingGardensWonder : BaseWonder
    {
        public HangingGardensWonder(WonderBoardSide boardSide)
            : base(boardSide)
        {
        }

        public override WonderName Name
        {
            get { return WonderName.HangingGardensOfBabylon; }
        }

        public override ResourceType OriginalResource
        {
            get { return ResourceType.Clay; }
        }

        protected override void CreateASideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 3) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Wood },
                new List<IEffect> { new Effect(EffectType.Tablet), new Effect(EffectType.Compass), new Effect(EffectType.Gear) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Clay, ResourceType.Clay },
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 7) });
        }

        protected override void CreateBSideStages()
        {
            InitializeStages(3);

            Stages[0].AddCostsAndEffects(new List<ResourceType> { ResourceType.Clay, ResourceType.Loom},
                new List<IEffect> { new Effect(EffectType.VictoryPoint, 3) });
            Stages[1].AddCostsAndEffects(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Glass },
                new List<IEffect> { new Effect(EffectType.PlaySeventhCard) });
            Stages[2].AddCostsAndEffects(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Clay, ResourceType.Papyrus },
                new List<IEffect> { new Effect(EffectType.Tablet), new Effect(EffectType.Compass), new Effect(EffectType.Gear) });
        }

        public override IList<IList<IEffect>> ChoosableEffectsAvailable
        {
            get
            {
                var result = new List<IList<IEffect>>();
                result.Add(effectsInUse.Where(e => Enumerator.ContainsEnumeratorValue<ScientificSymbol>((int)e.Type)).ToList());
                return result;
            }
        }

        public override IList<IEffect> EffectsAvailable
        {
            get
            {
                return effectsInUse.Where(e => !Enumerator.ContainsEnumeratorValue<ScientificSymbol>((int)e.Type)).ToList();
            }
        }
    }
}
