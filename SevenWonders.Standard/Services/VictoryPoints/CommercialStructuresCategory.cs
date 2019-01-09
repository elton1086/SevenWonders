using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWonders.Services.VictoryPoints
{
    public class CommercialStructuresCategory : PointsCategory
    {
        private readonly ICoreLogger logger;

        public CommercialStructuresCategory(ICoreLogger logger)
        {
            this.logger = logger;
        }

        protected override void Compute(IList<Player> players)
        {
            logger.Debug("Starting to compute commercial structure victory points");
            foreach (var p in players)
            {
                var cards = p.Cards.Where(c => c.Type == BaseEntities.StructureType.Commercial).ToList();
                var sb = new StringBuilder();

                foreach (var c in cards)
                {
                    foreach (var e in c.StandaloneEffect)
                    {
                        var pts = 0;
                        switch (e.Type)
                        {
                            case BaseEntities.EffectType.VictoryPointPerWonderStageBuilt:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByWonderStage(p, e.Quantity);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerCommercialCard:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByStructureType(p, e.Quantity, StructureType.Commercial);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerRawMaterialCard:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByStructureType(p, e.Quantity, StructureType.RawMaterial);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerManufacturedGoodCard:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByStructureType(p, e.Quantity, StructureType.ManufacturedGood);
                                break;
                            default:
                                break;
                        }
                        if (pts > 0)
                            sb.AppendFormat("{0}({1}) ", c.Name, pts);
                    }
                }

                logger.Debug("{0} has now {1} victory points. {2}", p.Name, p.VictoryPoints, sb.ToString());
            }
        }
    }
}
