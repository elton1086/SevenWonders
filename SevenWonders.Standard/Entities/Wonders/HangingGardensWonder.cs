using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Entities
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

        protected override IList<WonderStage> CreateASideStages()
        {
            return new List<WonderStage>
            {
                new WonderStage(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 3) }),
                new WonderStage(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Wood },
                    new List<Effect> { new Effect(EffectType.Tablet), new Effect(EffectType.Compass), new Effect(EffectType.Gear) }),
                new WonderStage(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Clay, ResourceType.Clay },
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 7) })
            };
        }

        protected override IList<WonderStage> CreateBSideStages()
        {
            return new List<WonderStage>
            {
                new WonderStage(new List<ResourceType> { ResourceType.Clay, ResourceType.Loom},
                    new List<Effect> { new Effect(EffectType.VictoryPoint, 3) }),
                new WonderStage(new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Glass },
                    new List<Effect> { new Effect(EffectType.PlaySeventhCard) }),
                new WonderStage(new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Clay, ResourceType.Papyrus },
                    new List<Effect> { new Effect(EffectType.Tablet), new Effect(EffectType.Compass), new Effect(EffectType.Gear) })
            };
        }

        public override IList<IList<Effect>> ChoosableEffectsAvailable
        {
            get
            {
                var result = new List<IList<Effect>>();
                result.Add(effectsInUse.Where(e => Enumerator.ContainsEnumeratorValue<ScientificSymbol>((int)e.Type)).ToList());
                return result;
            }
        }

        public override IList<Effect> EffectsAvailable
        {
            get
            {
                return effectsInUse.Where(e => !Enumerator.ContainsEnumeratorValue<ScientificSymbol>((int)e.Type)).ToList();
            }
        }
    }
}
