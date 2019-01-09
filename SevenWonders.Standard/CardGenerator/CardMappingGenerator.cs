using SevenWonders.BaseEntities;
using SevenWonders.Entities;
using System.Collections.Generic;

namespace SevenWonders.CardGenerator
{
    public static class CardMappingGenerator
    {
        public static CardCollection GenerateBaseGameCards()
        {
            var collection = new CardCollection();
            CreateAgeICards(collection.CardMapping);
            CreateAgeIICards(collection.CardMapping);
            CreateAgeIIICards(collection.CardMapping);
            return collection;
        }

        private static void CreateAgeICards(IList<CardMapping> mappings)
        {
            if (mappings == null)
                return;
            var age = Age.I;
            #region Raw Materials
            var type = StructureType.RawMaterial;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.LumberYard,
                Effects = new List<Effect> { new Effect(EffectType.Wood) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.LumberYard,
                Effects = new List<Effect> { new Effect(EffectType.Wood) },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.OreVein,
                Effects = new List<Effect> { new Effect(EffectType.Ore) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.OreVein,
                Effects = new List<Effect> { new Effect(EffectType.Ore) },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.ClayPool,
                Effects = new List<Effect> { new Effect(EffectType.Clay) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.ClayPool,
                Effects = new List<Effect> { new Effect(EffectType.Clay) },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.StonePit,
                Effects = new List<Effect> { new Effect(EffectType.Stone) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.StonePit,
                Effects = new List<Effect> { new Effect(EffectType.Stone) },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.TimberYard,
                Effects = new List<Effect> { new Effect(EffectType.Stone), new Effect(EffectType.Wood) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.ClayPit,
                Effects = new List<Effect> { new Effect(EffectType.Ore), new Effect(EffectType.Clay) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Excavation,
                Effects = new List<Effect> { new Effect(EffectType.Stone), new Effect(EffectType.Clay) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.ForestCave,
                Effects = new List<Effect> { new Effect(EffectType.Ore), new Effect(EffectType.Wood) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.TreeFarm,
                Effects = new List<Effect> { new Effect(EffectType.Wood), new Effect(EffectType.Clay) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Mine,
                Effects = new List<Effect> { new Effect(EffectType.Stone), new Effect(EffectType.Ore) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 6
            });
            #endregion

            #region Manufactured Goods
            type = StructureType.ManufacturedGood;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Loom,
                Effects = new List<Effect> { new Effect(EffectType.Loom) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Loom,
                Effects = new List<Effect> { new Effect(EffectType.Loom) },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Glassworks,
                Effects = new List<Effect> { new Effect(EffectType.Glass) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Glassworks,
                Effects = new List<Effect> { new Effect(EffectType.Glass) },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Press,
                Effects = new List<Effect> { new Effect(EffectType.Papyrus) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Press,
                Effects = new List<Effect> { new Effect(EffectType.Papyrus) },
                MinimumPlayers = 6
            });
            #endregion

            #region Civilian
            type = StructureType.Civilian;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Altar,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 2) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Altar,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 2) },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Theatre,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 2) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Theatre,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 2) },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Baths,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Baths,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Pawnshop,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 3) },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Pawnshop,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 3) },
                MinimumPlayers = 7
            });
            #endregion

            #region Commercial
            type = StructureType.Commercial;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.EastTradingPost,
                Effects = new List<Effect> { new Effect(EffectType.BuyRawMaterialDiscount, 1, PlayerDirection.ToTheRight) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.EastTradingPost,
                Effects = new List<Effect> { new Effect(EffectType.BuyRawMaterialDiscount, 1, PlayerDirection.ToTheRight) },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.WestTradingPost,
                Effects = new List<Effect> { new Effect(EffectType.BuyRawMaterialDiscount, 1, PlayerDirection.ToTheLeft) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.WestTradingPost,
                Effects = new List<Effect> { new Effect(EffectType.BuyRawMaterialDiscount, 1, PlayerDirection.ToTheLeft) },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Marketplace,
                Effects = new List<Effect> { new Effect(EffectType.BuyManufacturedGoodDiscount, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Marketplace,
                Effects = new List<Effect> { new Effect(EffectType.BuyManufacturedGoodDiscount, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Tavern,
                Effects = new List<Effect> { new Effect(EffectType.Coin, 5) },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Tavern,
                Effects = new List<Effect> { new Effect(EffectType.Coin, 5) },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Tavern,
                Effects = new List<Effect> { new Effect(EffectType.Coin, 5) },
                MinimumPlayers = 7
            });
            #endregion

            #region Military
            type = StructureType.Military;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Stockade,
                Effects = new List<Effect> { new Effect(EffectType.Shield) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Stockade,
                Effects = new List<Effect> { new Effect(EffectType.Shield) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Barracks,
                Effects = new List<Effect> { new Effect(EffectType.Shield) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Barracks,
                Effects = new List<Effect> { new Effect(EffectType.Shield) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.GuardTower,
                Effects = new List<Effect> { new Effect(EffectType.Shield) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.GuardTower,
                Effects = new List<Effect> { new Effect(EffectType.Shield) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay },
                MinimumPlayers = 4
            });
            #endregion

            #region Scientific
            type = StructureType.Scientific;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Apothecary,
                Effects = new List<Effect> { new Effect(EffectType.Compass) },
                ResourceCosts = new List<ResourceType> { ResourceType.Loom },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Apothecary,
                Effects = new List<Effect> { new Effect(EffectType.Compass) },
                ResourceCosts = new List<ResourceType> { ResourceType.Loom },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Workshop,
                Effects = new List<Effect> { new Effect(EffectType.Gear) },
                ResourceCosts = new List<ResourceType> { ResourceType.Glass },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Workshop,
                Effects = new List<Effect> { new Effect(EffectType.Gear) },
                ResourceCosts = new List<ResourceType> { ResourceType.Glass },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Scriptorium,
                Effects = new List<Effect> { new Effect(EffectType.Tablet) },
                ResourceCosts = new List<ResourceType> { ResourceType.Papyrus },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Scriptorium,
                Effects = new List<Effect> { new Effect(EffectType.Tablet) },
                ResourceCosts = new List<ResourceType> { ResourceType.Papyrus },
                MinimumPlayers = 4
            });
            #endregion
        }

        private static void CreateAgeIICards(IList<CardMapping> mappings)
        {
            if (mappings == null)
                return;
            var age = Age.II;
            #region Raw Materials
            var type = StructureType.RawMaterial;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Sawmill,
                Effects = new List<Effect> { new Effect(EffectType.Wood, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Sawmill,
                Effects = new List<Effect> { new Effect(EffectType.Wood, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Foundry,
                Effects = new List<Effect> { new Effect(EffectType.Ore, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Foundry,
                Effects = new List<Effect> { new Effect(EffectType.Ore, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Brickyard,
                Effects = new List<Effect> { new Effect(EffectType.Clay, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Brickyard,
                Effects = new List<Effect> { new Effect(EffectType.Clay, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Quarry,
                Effects = new List<Effect> { new Effect(EffectType.Stone, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Quarry,
                Effects = new List<Effect> { new Effect(EffectType.Stone, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Coin },
                MinimumPlayers = 4
            });
            #endregion

            #region Manufactured Goods
            type = StructureType.ManufacturedGood;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Loom,
                Effects = new List<Effect> { new Effect(EffectType.Loom) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Loom,
                Effects = new List<Effect> { new Effect(EffectType.Loom) },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Glassworks,
                Effects = new List<Effect> { new Effect(EffectType.Glass) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Glassworks,
                Effects = new List<Effect> { new Effect(EffectType.Glass) },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Press,
                Effects = new List<Effect> { new Effect(EffectType.Papyrus) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Press,
                Effects = new List<Effect> { new Effect(EffectType.Papyrus) },
                MinimumPlayers = 5
            });
            #endregion

            #region Civilian
            type = StructureType.Civilian;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Temple,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Clay, ResourceType.Glass },
                CardCosts = new List<CardName> { CardName.Altar },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Temple,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Clay, ResourceType.Glass },
                CardCosts = new List<CardName> { CardName.Altar },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Courthouse,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 4) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Loom },
                CardCosts = new List<CardName> { CardName.Scriptorium },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Courthouse,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 4) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Loom },
                CardCosts = new List<CardName> { CardName.Scriptorium },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Statue,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 4) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Ore, ResourceType.Ore },
                CardCosts = new List<CardName> { CardName.Theatre },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Statue,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 4) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Ore, ResourceType.Ore },
                CardCosts = new List<CardName> { CardName.Theatre },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Aqueduct,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 5) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone },
                CardCosts = new List<CardName> { CardName.Baths },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Aqueduct,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 5) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone },
                CardCosts = new List<CardName> { CardName.Baths },
                MinimumPlayers = 7
            });
            #endregion

            #region Commercial
            type = StructureType.Commercial;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Caravansery,
                Effects = new List<Effect> { new Effect(EffectType.Clay), new Effect(EffectType.Stone), new Effect(EffectType.Wood), new Effect(EffectType.Ore) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Marketplace },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Caravansery,
                Effects = new List<Effect> { new Effect(EffectType.Clay), new Effect(EffectType.Stone), new Effect(EffectType.Wood), new Effect(EffectType.Ore) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Marketplace },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Caravansery,
                Effects = new List<Effect> { new Effect(EffectType.Clay), new Effect(EffectType.Stone), new Effect(EffectType.Wood), new Effect(EffectType.Ore) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Marketplace },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Forum,
                Effects = new List<Effect> { new Effect(EffectType.Loom), new Effect(EffectType.Glass), new Effect(EffectType.Papyrus) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay },
                CardCosts = new List<CardName> { CardName.EastTradingPost, CardName.WestTradingPost },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Forum,
                Effects = new List<Effect> { new Effect(EffectType.Loom), new Effect(EffectType.Glass), new Effect(EffectType.Papyrus) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay },
                CardCosts = new List<CardName> { CardName.EastTradingPost, CardName.WestTradingPost },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Forum,
                Effects = new List<Effect> { new Effect(EffectType.Loom), new Effect(EffectType.Glass), new Effect(EffectType.Papyrus) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay },
                CardCosts = new List<CardName> { CardName.EastTradingPost, CardName.WestTradingPost },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Vineyard,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerRawMaterialCard, 1, PlayerDirection.Myself | PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Vineyard,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerRawMaterialCard, 1, PlayerDirection.Myself | PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Bazaar,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerManufacturedGoodCard, 2, PlayerDirection.Myself | PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Bazaar,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerManufacturedGoodCard, 2, PlayerDirection.Myself | PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                MinimumPlayers = 7
            });
            #endregion

            #region Military
            type = StructureType.Military;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Stables,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Clay, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Apothecary },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Stables,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Clay, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Apothecary },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.ArcheryRanch,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Ore },
                CardCosts = new List<CardName> { CardName.Workshop },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.ArcheryRanch,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Ore },
                CardCosts = new List<CardName> { CardName.Workshop },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Walls,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Walls,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.TrainningGround,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Wood },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.TrainningGround,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Wood },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.TrainningGround,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 2) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Wood },
                MinimumPlayers = 7
            });
            #endregion

            #region Scientific
            type = StructureType.Scientific;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Library,
                Effects = new List<Effect> { new Effect(EffectType.Tablet) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Loom },
                CardCosts = new List<CardName> { CardName.Scriptorium },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Library,
                Effects = new List<Effect> { new Effect(EffectType.Tablet) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Loom },
                CardCosts = new List<CardName> { CardName.Scriptorium },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Laboratory,
                Effects = new List<Effect> { new Effect(EffectType.Gear) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Papyrus },
                CardCosts = new List<CardName> { CardName.Workshop },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Laboratory,
                Effects = new List<Effect> { new Effect(EffectType.Gear) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Papyrus },
                CardCosts = new List<CardName> { CardName.Workshop },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Dispensary,
                Effects = new List<Effect> { new Effect(EffectType.Compass) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Glass },
                CardCosts = new List<CardName> { CardName.Apothecary },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Dispensary,
                Effects = new List<Effect> { new Effect(EffectType.Compass) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Glass },
                CardCosts = new List<CardName> { CardName.Apothecary },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.School,
                Effects = new List<Effect> { new Effect(EffectType.Tablet) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Papyrus },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.School,
                Effects = new List<Effect> { new Effect(EffectType.Tablet) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Papyrus },
                MinimumPlayers = 7
            });
            #endregion
        }

        private static void CreateAgeIIICards(IList<CardMapping> mappings)
        {
            if (mappings == null)
                return;
            var age = Age.III;
            #region Civilian
            var type = StructureType.Civilian;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Gardens,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 5) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Statue },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Gardens,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 5) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Statue },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Senate,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 6) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Stone, ResourceType.Wood, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Library },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Senate,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 6) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Stone, ResourceType.Wood, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Library },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.TownHall,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 6) },
                ResourceCosts = new List<ResourceType> { ResourceType.Glass, ResourceType.Ore, ResourceType.Stone, ResourceType.Stone },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.TownHall,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 6) },
                ResourceCosts = new List<ResourceType> { ResourceType.Glass, ResourceType.Ore, ResourceType.Stone, ResourceType.Stone },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.TownHall,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 6) },
                ResourceCosts = new List<ResourceType> { ResourceType.Glass, ResourceType.Ore, ResourceType.Stone, ResourceType.Stone },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Pantheon,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 7) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Ore, ResourceType.Loom, ResourceType.Glass, ResourceType.Papyrus },
                CardCosts = new List<CardName> { CardName.Temple },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Pantheon,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 7) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Ore, ResourceType.Loom, ResourceType.Glass, ResourceType.Papyrus },
                CardCosts = new List<CardName> { CardName.Temple },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Palace,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 8) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Wood, ResourceType.Ore, ResourceType.Stone, ResourceType.Loom, ResourceType.Glass, ResourceType.Papyrus },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Palace,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPoint, 8) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Wood, ResourceType.Ore, ResourceType.Stone, ResourceType.Loom, ResourceType.Glass, ResourceType.Papyrus },
                MinimumPlayers = 7
            });
            #endregion

            #region Commercial
            type = StructureType.Commercial;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Arena,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerWonderStageBuilt, 3, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerWonderStageBuilt, 1, PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Ore },
                CardCosts = new List<CardName> { CardName.Dispensary },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Arena,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerWonderStageBuilt, 3, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerWonderStageBuilt, 1, PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Ore },
                CardCosts = new List<CardName> { CardName.Dispensary },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Arena,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerWonderStageBuilt, 3, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerWonderStageBuilt, 1, PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Ore },
                CardCosts = new List<CardName> { CardName.Dispensary },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Lighthouse,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerCommercialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerCommercialCard, 1, PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Glass, ResourceType.Stone },
                CardCosts = new List<CardName> { CardName.Caravansery },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Lighthouse,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerCommercialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerCommercialCard, 1, PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Glass, ResourceType.Stone },
                CardCosts = new List<CardName> { CardName.Caravansery },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Haven,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerRawMaterialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerRawMaterialCard, 1, PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Loom, ResourceType.Ore, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Forum },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Haven,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerRawMaterialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerRawMaterialCard, 1, PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Loom, ResourceType.Ore, ResourceType.Wood },
                CardCosts = new List<CardName> { CardName.Forum },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.ChamberOfCommerce,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerManufacturedGoodCard, 2, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerManufacturedGoodCard, 2, PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Papyrus },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.ChamberOfCommerce,
                Effects = new List<Effect> { new Effect(EffectType.CoinPerManufacturedGoodCard, 2, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerManufacturedGoodCard, 2, PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Papyrus },
                MinimumPlayers = 6
            });
            #endregion

            #region Military
            type = StructureType.Military;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.SiegeWorkshop,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Clay, ResourceType.Clay, ResourceType.Clay },
                CardCosts = new List<CardName> { CardName.Laboratory },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.SiegeWorkshop,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Clay, ResourceType.Clay, ResourceType.Clay },
                CardCosts = new List<CardName> { CardName.Laboratory },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Fortification,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Ore, ResourceType.Stone },
                CardCosts = new List<CardName> { CardName.Walls },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Fortification,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Ore, ResourceType.Stone },
                CardCosts = new List<CardName> { CardName.Walls },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Arsenal,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Wood, ResourceType.Wood, ResourceType.Loom },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Arsenal,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Wood, ResourceType.Wood, ResourceType.Loom },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Arsenal,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Wood, ResourceType.Wood, ResourceType.Loom },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Circus,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Ore },
                CardCosts = new List<CardName> { CardName.TrainningGround },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Circus,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Ore },
                CardCosts = new List<CardName> { CardName.TrainningGround },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Circus,
                Effects = new List<Effect> { new Effect(EffectType.Shield, 3) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Ore },
                CardCosts = new List<CardName> { CardName.TrainningGround },
                MinimumPlayers = 6
            });
            #endregion

            #region Scientific
            type = StructureType.Scientific;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.University,
                Effects = new List<Effect> { new Effect(EffectType.Tablet) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Papyrus, ResourceType.Glass },
                CardCosts = new List<CardName> { CardName.Library },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.University,
                Effects = new List<Effect> { new Effect(EffectType.Tablet) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Papyrus, ResourceType.Glass },
                CardCosts = new List<CardName> { CardName.Library },
                MinimumPlayers = 4
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Observatory,
                Effects = new List<Effect> { new Effect(EffectType.Gear) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Loom, ResourceType.Glass },
                CardCosts = new List<CardName> { CardName.Laboratory },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Observatory,
                Effects = new List<Effect> { new Effect(EffectType.Gear) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Loom, ResourceType.Glass },
                CardCosts = new List<CardName> { CardName.Laboratory },
                MinimumPlayers = 7
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Lodge,
                Effects = new List<Effect> { new Effect(EffectType.Compass) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Loom, ResourceType.Papyrus },
                CardCosts = new List<CardName> { CardName.Dispensary },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Lodge,
                Effects = new List<Effect> { new Effect(EffectType.Compass) },
                ResourceCosts = new List<ResourceType> { ResourceType.Clay, ResourceType.Clay, ResourceType.Loom, ResourceType.Papyrus },
                CardCosts = new List<CardName> { CardName.Dispensary },
                MinimumPlayers = 6
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Study,
                Effects = new List<Effect> { new Effect(EffectType.Gear) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Loom, ResourceType.Papyrus },
                CardCosts = new List<CardName> { CardName.School },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Study,
                Effects = new List<Effect> { new Effect(EffectType.Gear) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Loom, ResourceType.Papyrus },
                CardCosts = new List<CardName> { CardName.School },
                MinimumPlayers = 5
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Academy,
                Effects = new List<Effect> { new Effect(EffectType.Compass) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Glass },
                CardCosts = new List<CardName> { CardName.School },
                MinimumPlayers = 3
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.Academy,
                Effects = new List<Effect> { new Effect(EffectType.Compass) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Stone, ResourceType.Glass },
                CardCosts = new List<CardName> { CardName.School },
                MinimumPlayers = 7
            });
            #endregion

            #region Guilds
            type = StructureType.Guilds;
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.WorkersGuild,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPointPerRawMaterialCard, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Stone, ResourceType.Clay, ResourceType.Wood }
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.CraftsmenGuild,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPointPerManufacturedGoodCard, 2, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Stone, ResourceType.Stone }
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.TradersGuild,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPointPerCommercialCard, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                ResourceCosts = new List<ResourceType> { ResourceType.Loom, ResourceType.Glass, ResourceType.Papyrus }
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.PhilosophersGuild,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPointPerScientificCard, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                ResourceCosts = new List<ResourceType> { ResourceType.Loom, ResourceType.Papyrus, ResourceType.Clay, ResourceType.Clay, ResourceType.Clay }
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.SpiesGuild,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPointPerMilitaryCard, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                ResourceCosts = new List<ResourceType> { ResourceType.Glass, ResourceType.Clay, ResourceType.Clay, ResourceType.Clay }
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.StrategistsGuild,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPointPerConflictDefeat, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Stone, ResourceType.Loom }
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.ShipownersGuild,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPointPerRawMaterialCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerManufacturedGoodCard, 1, PlayerDirection.Myself), new Effect(EffectType.VictoryPointPerGuildCard, 1, PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Wood, ResourceType.Papyrus, ResourceType.Glass }
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.ScientistsGuild,
                Effects = new List<Effect> { new Effect(EffectType.Compass), new Effect(EffectType.Gear), new Effect(EffectType.Tablet) },
                ResourceCosts = new List<ResourceType> { ResourceType.Ore, ResourceType.Ore, ResourceType.Wood, ResourceType.Wood, ResourceType.Papyrus }
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.MagistratesGuild,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPointPerCivilianCard, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight) },
                ResourceCosts = new List<ResourceType> { ResourceType.Wood, ResourceType.Wood, ResourceType.Wood, ResourceType.Stone, ResourceType.Loom }
            });
            mappings.Add(new CardMapping
            {
                Age = age,
                Type = type,
                Name = CardName.BuildersGuild,
                Effects = new List<Effect> { new Effect(EffectType.VictoryPointPerWonderStageBuilt, 1, PlayerDirection.ToTheLeft | PlayerDirection.ToTheRight | PlayerDirection.Myself) },
                ResourceCosts = new List<ResourceType> { ResourceType.Stone, ResourceType.Stone, ResourceType.Clay, ResourceType.Clay, ResourceType.Glass }
            });
            #endregion
        }
    }
}
