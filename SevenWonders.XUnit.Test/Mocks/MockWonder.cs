using SevenWonders.BaseEntities;
using SevenWonders.Entities;
using System.Collections.Generic;

namespace SevenWonders.XUnit.Test.Mocks
{
    public class MockWonder : BaseWonder
    {
        public override WonderName Name { get; }
        public override ResourceType OriginalResource { get { return UseResource; } }

        public ResourceType UseResource { get; set; }

        public MockWonder() : base(WonderBoardSide.A) { }

        public void SetStages(IList<WonderStage> stages)
        {
            base.stages = stages;
        }

        protected override IList<WonderStage> CreateASideStages()
        {
            return stages;
        }

        protected override IList<WonderStage> CreateBSideStages()
        {
            return stages;
        }
    }
}
