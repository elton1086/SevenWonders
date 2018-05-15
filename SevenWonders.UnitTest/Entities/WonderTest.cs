using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenWonder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.UnitTest.Entities
{
    [TestClass]
    public class WonderTest
    {
        [TestMethod]
        public void HangingGardensWonderScientificChoiceTest()
        {
            var wonder = new HangingGardensWonder(SevenWonder.BaseEntities.WonderBoardSide.A);
            wonder.BuildStage();
            wonder.BuildStage();
            Assert.IsTrue(wonder.ChoosableEffectsAvailable.Any(v => v.Any(e => e.Type == SevenWonder.BaseEntities.EffectType.Tablet)));
        }

        [TestMethod]
        public void LastStageBuiltTest()
        {
            var wonder = new PyramidsWonder(SevenWonder.BaseEntities.WonderBoardSide.B);
            wonder.BuildStage();
            wonder.BuildStage();
            wonder.BuildStage();
            wonder.BuildStage();
            Assert.IsTrue(wonder.NextStage == null);
            Assert.AreEqual(4, wonder.StagesBuilt);
        }
    }
}
