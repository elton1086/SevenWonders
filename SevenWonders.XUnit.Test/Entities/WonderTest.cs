using SevenWonders.Entities;
using System.Linq;
using Xunit;

namespace SevenWonders.UnitTest.Entities
{
    public class WonderTest
    {
        [Fact]
        public void HangingGardensWonderScientificChoiceTest()
        {
            var wonder = new HangingGardensWonder(BaseEntities.WonderBoardSide.A);
            wonder.BuildStage();
            wonder.BuildStage();
            Assert.Contains(wonder.ChoosableEffectsAvailable, v => v.Any(e => e.Type == BaseEntities.EffectType.Tablet));
        }

        [Fact]
        public void LastStageBuiltTest()
        {
            var wonder = new PyramidsWonder(BaseEntities.WonderBoardSide.B);
            wonder.BuildStage();
            wonder.BuildStage();
            wonder.BuildStage();
            wonder.BuildStage();
            Assert.Null(wonder.NextStage);
            Assert.Equal(4, wonder.StagesBuilt);
        }
    }
}
