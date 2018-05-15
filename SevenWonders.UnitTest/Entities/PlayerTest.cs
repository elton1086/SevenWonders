using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenWonder.Contracts;
using SevenWonder.Entities;
using SevenWonder.BaseEntities;
using SevenWonder.Helper;
using SevenWonder.CardGenerator;
using System.IO;
using System.Linq;

namespace SevenWonders.UnitTest.Entities
{
    /// <summary>
    /// Summary description for Player
    /// </summary>
    [TestClass]
    public class PlayerTest
    {
        IGamePlayer player;

        public PlayerTest()
        {
            player = new TurnPlayer("joão");
        }

        private void AddCards(string fileName)
        {
            foreach (var card in CardMappingGenerator.ReadAndMapXmlFile(Path.Combine(Environment.CurrentDirectory, "Files", fileName + ".xml")))
                player.Cards.Add(card);
        }

        [TestMethod]
        public void CheckResourceAvailabilityTest()
        {   
            AddCards("CheckResourceAvailability");
            player.SetWonder(new MausoleumWonder(WonderBoardSide.A));

            var neededResources = new List<ResourceType>();
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Glass);
            neededResources.Add(ResourceType.Papyrus);

            var result = player.CheckResourceAvailability(neededResources, false);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void CheckResourceAvailabilityShareableMissingPapyrusTest()
        {
            AddCards("CheckResourceAvailability");
            player.SetWonder(new MausoleumWonder(WonderBoardSide.A));

            var neededResources = new List<ResourceType>();
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Loom);
            neededResources.Add(ResourceType.Papyrus);

            var result = player.CheckResourceAvailability(neededResources, true);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Contains(ResourceType.Papyrus));
        }

        [TestMethod]
        public void CheckResourceAvailabilityUseWonderEffectsTest()
        {
            AddCards("CheckResourceAvailability");
            player.SetWonder(new LighthouseWonder(WonderBoardSide.A));
            player.Wonder.BuildStage();
            player.Wonder.BuildStage();

            var neededResources = new List<ResourceType>();
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Glass);
            neededResources.Add(ResourceType.Stone);

            var result = player.CheckResourceAvailability(neededResources, false);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void CheckResourceAvailabilityTooManyTest()
        {
            AddCards("CheckResourceAvailability");
            player.SetWonder(new LighthouseWonder(WonderBoardSide.A));
            player.Wonder.BuildStage();

            var neededResources = new List<ResourceType>();
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Papyrus);
            neededResources.Add(ResourceType.Glass);
            neededResources.Add(ResourceType.Glass);
            neededResources.Add(ResourceType.Glass);
            neededResources.Add(ResourceType.Ore);
            neededResources.Add(ResourceType.Ore);
            neededResources.Add(ResourceType.Wood);

            var result = player.CheckResourceAvailability(neededResources, false);
            Assert.IsTrue(result.Count == 3);
            Assert.IsTrue(result.Contains(ResourceType.Clay));
            Assert.IsTrue(result.Contains(ResourceType.Papyrus));
            Assert.IsTrue(result.Contains(ResourceType.Ore));
        }

        [TestMethod]
        public void CheckResourceAvailabilityTooManyShareableTest()
        {
            AddCards("CheckResourceAvailability");
            player.SetWonder(new MausoleumWonder(WonderBoardSide.B));

            var neededResources = new List<ResourceType>();
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Papyrus);
            neededResources.Add(ResourceType.Glass);
            neededResources.Add(ResourceType.Glass);
            neededResources.Add(ResourceType.Wood);
            neededResources.Add(ResourceType.Ore);
            neededResources.Add(ResourceType.Ore);

            var result = player.CheckResourceAvailability(neededResources, true);
            Assert.IsTrue(result.Count == 4);
            Assert.IsTrue(result.Contains(ResourceType.Clay));
            Assert.IsTrue(result.Contains(ResourceType.Glass));
            Assert.IsTrue(result.Contains(ResourceType.Papyrus));
            Assert.IsTrue(result.Contains(ResourceType.Ore));
        }

        [TestMethod]
        public void HasDicountTrueTest()
        {
            AddCards("HasDiscount");
            player.SetWonder(new PyramidsWonder(WonderBoardSide.B));
            var resultLeft = player.HasDiscount(PlayerDirection.ToTheLeft, TradeDiscountType.RawMaterial);
            var resultRight = player.HasDiscount(PlayerDirection.ToTheRight, TradeDiscountType.RawMaterial);
            Assert.IsTrue(resultLeft);
            Assert.IsFalse(resultRight);
        }

        [TestMethod]
        public void HasDicountFalseForMaterialTypeTest()
        {
            AddCards("HasDiscount");
            player.SetWonder(new PyramidsWonder(WonderBoardSide.B));
            var result = player.HasDiscount(PlayerDirection.ToTheLeft, TradeDiscountType.ManufacturedGood);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasDicountUseWonderBoardTest()
        {
            AddCards("HasDiscount");
            player.SetWonder(new StatueOfZeusWonder(WonderBoardSide.B));
            player.Wonder.BuildStage();
            var resultLeft = player.HasDiscount(PlayerDirection.ToTheLeft, TradeDiscountType.RawMaterial);
            var resultRight = player.HasDiscount(PlayerDirection.ToTheRight, TradeDiscountType.RawMaterial);
            Assert.IsTrue(resultLeft && resultRight);
        }

        [TestMethod]
        public void HasDicountBothTest()
        {
            AddCards("HasDiscountBoth");
            player.SetWonder(new PyramidsWonder(WonderBoardSide.B));
            var resultLeft = player.HasDiscount(PlayerDirection.ToTheLeft, TradeDiscountType.ManufacturedGood);
            var resultRight = player.HasDiscount(PlayerDirection.ToTheRight, TradeDiscountType.ManufacturedGood);
            Assert.IsTrue(resultLeft && resultRight);
        }
    }
}
