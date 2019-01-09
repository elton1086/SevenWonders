using SevenWonders.BaseEntities;
using SevenWonders.CardGenerator;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using System.Linq;

namespace SevenWonders.Factories
{
    public static class StructureCardFactory
    {
        public static StructureCard CreateStructureCard(CardMapping cardMapping)
        {
            switch (cardMapping.Type)
            {
                case StructureType.RawMaterial:
                    return new RawMaterialCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<Effect>());
                case StructureType.ManufacturedGood:
                    return new ManufacturedGoodCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<Effect>());
                case StructureType.Civilian:
                    return new CivilianCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<Effect>());
                case StructureType.Scientific:
                    return new ScientificCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<Effect>());
                case StructureType.Commercial:
                    return new CommercialCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<Effect>());
                case StructureType.Military:
                    return new MilitaryCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<Effect>());
                case StructureType.Guilds:
                    return new GuildCard(cardMapping.Name, cardMapping.MinimumPlayers, cardMapping.Age, cardMapping.ResourceCosts, cardMapping.CardCosts, cardMapping.Effects.ToList<Effect>());
                default:
                    return null;
            }
        }
    }
}
