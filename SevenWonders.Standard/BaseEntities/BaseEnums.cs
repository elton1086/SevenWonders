using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.BaseEntities
{
    public enum BasicType
    {
        Shield = 0,
        VictoryPoint,
        Coin,
    }

    /// <summary>
    /// The basic raw materials
    /// </summary>
    public enum RawMaterialType
    {
        /// <summary>
        /// Clay
        /// </summary>
        Clay = 10,
        /// <summary>
        /// Ore
        /// </summary>
        Ore,
        /// <summary>
        /// Stone
        /// </summary>
        Stone,
        /// <summary>
        /// Wood
        /// </summary>
        Wood,
    }

    /// <summary>
    /// The basic manufactured goods
    /// </summary>
    public enum ManufacturedGoodType
    {
        /// <summary>
        /// Glass
        /// </summary>
        Glass = 20,
        /// <summary>
        /// Loom
        /// </summary>
        Loom,
        /// <summary>
        /// Papyrus
        /// </summary>
        Papyrus,
    }

    /// <summary>
    /// The possible scientific symbols
    /// </summary>
    public enum ScientificSymbol
    {
        /// <summary>
        /// Table
        /// </summary>
        Tablet = 30,
        /// <summary>
        /// Compass
        /// </summary>
        Compass,
        /// <summary>
        /// Gear
        /// </summary>
        Gear,
    }

    /// <summary>
    /// Provides discount on the trade market with the neighbor players
    /// </summary>
    public enum TradeDiscountType
    {
        None,
        /// <summary>
        /// Pay only one coin per raw material
        /// </summary>
        RawMaterial = 40,
        /// <summary>
        /// Pay only one coin per manufactured good
        /// </summary>
        ManufacturedGood,
    }

    /// <summary>
    /// Receives coins immediately
    /// </summary>
    public enum CoinPaymentType
    {
        /// <summary>
        /// Coin per each raw material (brown) card
        /// </summary>
        RawMaterialCard = 50,
        /// <summary>
        /// Coin per each manufactured good (gray) card
        /// </summary>
        ManufacturedGoodCard,        
        /// <summary>
        /// Coin per each commercial (yellow) card
        /// </summary>
        CommercialCard,
        /// <summary>
        /// Coin per each wonder stage built
        /// </summary>
        WonderStageBuilt,
    }

    /// <summary>
    /// Receives victory point by the end of the game
    /// </summary>
    public enum VictoryPointType
    {
        /// <summary>
        /// VP for each raw material (brown) card
        /// </summary>
        RawMaterialCard = 60,
        /// <summary>
        /// VP for each manufactured good (gray) card
        /// </summary>
        ManufacturedGoodCard,
        /// <summary>
        /// VP for each commercial (yellow) card
        /// </summary>
        CommercialCard,
        /// <summary>
        /// VP for each civilian (blue) card
        /// </summary>
        CivilianCard,
        /// <summary>
        /// VP for each military (red) card
        /// </summary>
        MilitaryCard,
        /// <summary>
        /// VP for each scientific (green) card
        /// </summary>
        ScientificCard,
        /// <summary>
        /// VP for each guild (purple) card
        /// </summary>
        GuildCard,
        /// <summary>
        /// VP for each wonder stage built
        /// </summary>
        WonderStageBuilt,
        /// <summary>
        /// VP for each conflict defeat
        /// </summary>
        ConflictDefeat,
    }

    /// <summary>
    /// Special effect cases
    /// </summary>
    public enum SpecialCaseType
    {
        None,
        /// <summary>
        /// Can play the seventh card of each age
        /// </summary>
        PlaySeventhCard = 70,
        /// <summary>
        /// Look through discarded card deck and play one. It is played after the turn, so player can choose discarded cards during the last turn.
        /// </summary>
        PlayOneDiscardedCard,
        /// <summary>
        /// Once per age, can buy a card for free. Card cannot be sold or used to build a wonder stage. 
        /// </summary>
        PlayCardForFreeOncePerAge,
        /// <summary>
        /// At the end of the game, can copy a guild from one of the neighbors
        /// </summary>
        CopyGuildFromNeighbor,
    }

    public enum ResourceType
    {
        Coin = BasicType.Coin,
        Clay = RawMaterialType.Clay,
        Ore = RawMaterialType.Ore,
        Stone = RawMaterialType.Stone,
        Wood = RawMaterialType.Wood,
        Glass = ManufacturedGoodType.Glass,
        Loom = ManufacturedGoodType.Loom,
        Papyrus = ManufacturedGoodType.Papyrus,
    }

    public enum EffectType
    {
        Shield = BasicType.Shield,
        VictoryPoint = BasicType.VictoryPoint,
        Coin = ResourceType.Coin,
        Clay = ResourceType.Clay,
        Ore = ResourceType.Ore,
        Stone = ResourceType.Stone,
        Wood = ResourceType.Wood,
        Glass = ResourceType.Glass,
        Loom = ResourceType.Loom,
        Papyrus = ResourceType.Papyrus,
        Tablet = ScientificSymbol.Tablet,
        Compass = ScientificSymbol.Compass,
        Gear = ScientificSymbol.Gear,
        BuyRawMaterialDiscount = TradeDiscountType.RawMaterial,
        BuyManufacturedGoodDiscount = TradeDiscountType.ManufacturedGood,
        CoinPerRawMaterialCard = CoinPaymentType.RawMaterialCard,
        CoinPerManufacturedGoodCard = CoinPaymentType.ManufacturedGoodCard,        
        CoinPerCommercialCard = CoinPaymentType.CommercialCard,
        CoinPerWonderStageBuilt = CoinPaymentType.WonderStageBuilt,
        VictoryPointPerRawMaterialCard = VictoryPointType.RawMaterialCard,
        VictoryPointPerManufacturedGoodCard = VictoryPointType.ManufacturedGoodCard,
        VictoryPointPerCommercialCard = VictoryPointType.CommercialCard,
        VictoryPointPerCivilianCard = VictoryPointType.CivilianCard,
        VictoryPointPerMilitaryCard = VictoryPointType.MilitaryCard,
        VictoryPointPerScientificCard = VictoryPointType.ScientificCard,
        VictoryPointPerGuildCard = VictoryPointType.GuildCard,
        VictoryPointPerWonderStageBuilt = VictoryPointType.WonderStageBuilt,
        VictoryPointPerConflictDefeat = VictoryPointType.ConflictDefeat,
        PlaySeventhCard = SpecialCaseType.PlaySeventhCard,
        PlayOneDiscardedCard = SpecialCaseType.PlayOneDiscardedCard,
        PlayCardForFreeOncePerAge = SpecialCaseType.PlayCardForFreeOncePerAge,
        CopyGuildFromNeighbor = SpecialCaseType.CopyGuildFromNeighbor,
    }
}
