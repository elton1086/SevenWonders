using AutoFixture;
using AutoFixture.Xunit2;
using SevenWonders.BaseEntities;
using SevenWonders.CardGenerator;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Factories;
using SevenWonders.XUnit.Test.AutoData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace SevenWonders.UnitTest.Entities
{
    /// <summary>
    /// Summary description for Player
    /// </summary>
    public class PlayerTest
    {
        [Theory, AutoMoqData]
        public void CheckResourceAllAvailableTest([Frozen]IFixture fixture, GamePlayer player)
        {
            AddResourceAvailabilityTestCards(player, fixture);
            player.SetWonder(new MausoleumWonder(WonderBoardSide.A));

            var neededResources = new List<ResourceType>();
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Glass);
            neededResources.Add(ResourceType.Papyrus);

            var result = player.CheckResourceAvailability(neededResources, false);
            Assert.Empty(result);
        }

        [Theory, AutoMoqData]
        public void CheckResourceAvailabilityShareableMissingPapyrusTest([Frozen]IFixture fixture, GamePlayer player)
        {
            AddResourceAvailabilityTestCards(player, fixture);
            player.SetWonder(new MausoleumWonder(WonderBoardSide.A));

            var neededResources = new List<ResourceType>();
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Stone);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Clay);
            neededResources.Add(ResourceType.Loom);
            neededResources.Add(ResourceType.Papyrus);

            var result = player.CheckResourceAvailability(neededResources, true);
            Assert.Equal(1, result.Count);
            Assert.Contains(ResourceType.Papyrus, result);
        }

        [Theory, AutoMoqData]
        public void CheckResourceAvailabilityUseWonderEffectsTest([Frozen]IFixture fixture, GamePlayer player)
        {
            AddResourceAvailabilityTestCards(player, fixture);
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
            Assert.Empty(result);
        }

        [Theory, AutoMoqData]
        public void CheckResourceAvailabilityTooManyTest([Frozen]IFixture fixture, GamePlayer player)
        {
            AddResourceAvailabilityTestCards(player, fixture);
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
            Assert.Equal(3, result.Count);
            Assert.Contains(ResourceType.Clay, result);
            Assert.Contains(ResourceType.Papyrus, result);
            Assert.Contains(ResourceType.Ore, result);
        }

        [Theory, AutoMoqData]
        public void CheckResourceAvailabilityTooManyShareableTest([Frozen]IFixture fixture, GamePlayer player)
        {
            AddResourceAvailabilityTestCards(player, fixture);
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
            Assert.Equal(4, result.Count);
            Assert.Contains(ResourceType.Clay, result);
            Assert.Contains(ResourceType.Glass, result);
            Assert.Contains(ResourceType.Papyrus, result);
            Assert.Contains(ResourceType.Ore, result);
        }

        [Theory, AutoMoqData]
        public void HasDicountToOneSideOnlyForRawMaterialTypeTest([Frozen]IFixture fixture, GamePlayer player)
        {
            player.Cards.Add(StructureCardFactory.CreateStructureCard(fixture.Build<CardMapping>()
                .With(c => c.Type, StructureType.Commercial)
                .With(c => c.Effects, fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.BuyRawMaterialDiscount)
                    .With(e => e.Quantity, 1)
                    .With(e => e.Direction, PlayerDirection.ToTheLeft)
                    .CreateMany(1)
                    .ToList())
                .Create()));

            player.SetWonder(new PyramidsWonder(WonderBoardSide.B));
            Assert.True(player.HasDiscount(PlayerDirection.ToTheLeft, TradeDiscountType.RawMaterial));
            Assert.False(player.HasDiscount(PlayerDirection.ToTheRight, TradeDiscountType.RawMaterial));
            Assert.False(player.HasDiscount(PlayerDirection.ToTheRight, TradeDiscountType.RawMaterial));
        }

        [Theory, AutoMoqData]
        public void HasDicountBothTest([Frozen]IFixture fixture, GamePlayer player)
        {
            player.Cards.Add(StructureCardFactory.CreateStructureCard(fixture.Build<CardMapping>()
                .With(c => c.Type, StructureType.Commercial)
                .With(c => c.Effects, fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.BuyManufacturedGoodDiscount)
                    .With(e => e.Quantity, 1)
                    .With(e => e.Direction, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight)
                    .CreateMany(1)
                    .ToList())
                .Create()));

            player.SetWonder(new PyramidsWonder(WonderBoardSide.B));
            Assert.True(player.HasDiscount(PlayerDirection.ToTheLeft, TradeDiscountType.ManufacturedGood));
            Assert.True(player.HasDiscount(PlayerDirection.ToTheRight, TradeDiscountType.ManufacturedGood));
        }

        [Theory, AutoMoqData]
        public void HasDicountUseWonderBoardTest(GamePlayer player)
        {            
            player.SetWonder(new StatueOfZeusWonder(WonderBoardSide.B));
            player.Wonder.BuildStage();
            Assert.True(player.HasDiscount(PlayerDirection.ToTheLeft, TradeDiscountType.RawMaterial));
            Assert.True(player.HasDiscount(PlayerDirection.ToTheRight, TradeDiscountType.RawMaterial));
        }

        private void AddResourceAvailabilityTestCards(GamePlayer player, IFixture fixture)
        {
            player.Cards.Add(StructureCardFactory.CreateStructureCard(fixture.Build<CardMapping>()
                .With(c => c.Type, StructureType.RawMaterial)
                .With(c => c.Effects, new List<Effect>
                {
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Clay)
                    .With(e => e.Quantity, 1)
                    .Create()
                })
                .Create()));
            player.Cards.Add(StructureCardFactory.CreateStructureCard(fixture.Build<CardMapping>()
                .With(c => c.Type, StructureType.Commercial)
                .With(c => c.Effects, new List<Effect>
                {
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Loom)
                    .With(e => e.Quantity, 1)
                    .Create(),
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Glass)
                    .With(e => e.Quantity, 1)
                    .Create(),
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Papyrus)
                    .With(e => e.Quantity, 1)
                    .Create()
                })
                .Create()));
            player.Cards.Add(StructureCardFactory.CreateStructureCard(fixture.Build<CardMapping>()
                .With(c => c.Type, StructureType.RawMaterial)
                .With(c => c.Effects, new List<Effect>
                {
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Stone)
                    .With(e => e.Quantity, 1)
                    .Create(),
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Clay)
                    .With(e => e.Quantity, 1)
                    .Create()
                })
                .Create()));
            player.Cards.Add(StructureCardFactory.CreateStructureCard(fixture.Build<CardMapping>()
                .With(c => c.Type, StructureType.RawMaterial)
                .With(c => c.Effects, new List<Effect>
                {
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Wood)
                    .With(e => e.Quantity, 1)
                    .Create(),
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Ore)
                    .With(e => e.Quantity, 1)
                    .Create()
                })
                .Create()));
            player.Cards.Add(StructureCardFactory.CreateStructureCard(fixture.Build<CardMapping>()
                .With(c => c.Type, StructureType.RawMaterial)
                .With(c => c.Effects, new List<Effect>
                {
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Wood)
                    .With(e => e.Quantity, 1)
                    .Create(),
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Ore)
                    .With(e => e.Quantity, 1)
                    .Create()
                })
                .Create()));
            player.Cards.Add(StructureCardFactory.CreateStructureCard(fixture.Build<CardMapping>()
                .With(c => c.Type, StructureType.ManufacturedGood)
                .With(c => c.Effects, new List<Effect>
                {
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Glass)
                    .With(e => e.Quantity, 1)
                    .Create()
                })
                .Create()));
            player.Cards.Add(StructureCardFactory.CreateStructureCard(fixture.Build<CardMapping>()
                .With(c => c.Type, StructureType.RawMaterial)
                .With(c => c.Effects, new List<Effect>
                {
                    fixture.Build<Effect>()
                    .With(e => e.Type, EffectType.Stone)
                    .With(e => e.Quantity, 2)
                    .Create()
                })
                .Create()));
        }
    }
}
