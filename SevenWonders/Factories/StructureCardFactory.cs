using SevenWonder.BaseEntities;
using SevenWonder.CardGenerator;
using SevenWonder.Contracts;
using SevenWonder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonder.Factories
{
    public static class StructureCardFactory
    {
        public static IStructureCard CreateStructureCard(CardMapping cardMapping)
        {
            switch (cardMapping.Type)
            {
                case StructureType.RawMaterial:
                    return new RawMaterialCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<IEffect>());
                case StructureType.ManufacturedGood:
                    return new ManufacturedGoodCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<IEffect>());
                case StructureType.Civilian:
                    return new CivilianCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<IEffect>());
                case StructureType.Scientific:
                    return new ScientificCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<IEffect>());
                case StructureType.Commercial:
                    return new CommercialCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<IEffect>());
                case StructureType.Military:
                    return new MilitaryCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<IEffect>());
                case StructureType.Guilds:
                    return new GuildCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<IEffect>());
                default:
                    return new NullStructureCard();
            }
        }
    }
}
