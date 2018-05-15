using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWonder.Services.VictoryPoints
{
    public class GuildsCategory : PointsCategory
    {
        protected override void Compute(IList<IPlayer> players)
        {
            LoggerHelper.DebugFormat("Starting to compute guild victory points");
            foreach (var p in players)
            {
                var cards = p.Cards.Where(c => c.Type == BaseEntities.StructureType.Guilds).ToList();
                var neighbors = NeighborsHelper.GetNeighbors(players, p);
                var rightNeighbor = neighbors[NeighborsHelper.RIGHTDIRECTION];
                var leftNeighbor = neighbors[NeighborsHelper.LEFTDIRECTION];
                var sb = new StringBuilder();

                foreach (var c in cards)
                {
                    foreach (var e in c.StandaloneEffect)
                    {
                        var pts = 0;
                        switch (e.Type)
                        {
                            case BaseEntities.EffectType.VictoryPointPerRawMaterialCard:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByStructureType(p, e.Quantity, StructureType.RawMaterial, e.Direction, rightNeighbor, leftNeighbor);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerManufacturedGoodCard:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByStructureType(p, e.Quantity, StructureType.ManufacturedGood, e.Direction, rightNeighbor, leftNeighbor);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerCommercialCard:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByStructureType(p, e.Quantity, StructureType.Commercial, e.Direction, rightNeighbor, leftNeighbor);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerScientificCard:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByStructureType(p, e.Quantity, StructureType.Scientific, e.Direction, rightNeighbor, leftNeighbor);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerMilitaryCard:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByStructureType(p, e.Quantity, StructureType.Military, e.Direction, rightNeighbor, leftNeighbor);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerConflictDefeat:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByConflictDefeat(p, e.Quantity, StructureType.Military, e.Direction, rightNeighbor, leftNeighbor);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerCivilianCard:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByStructureType(p, e.Quantity, StructureType.Civilian, e.Direction, rightNeighbor, leftNeighbor);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerWonderStageBuilt:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByWonderStage(p, e.Quantity, e.Direction, rightNeighbor, leftNeighbor);
                                break;
                            case BaseEntities.EffectType.VictoryPointPerGuildCard:
                                p.VictoryPoints += pts = VictoryPointsHelper.GetVictoryPointsByStructureType(p, e.Quantity, StructureType.Guilds, e.Direction, rightNeighbor, leftNeighbor);
                                break;
                            default:
                                break;
                        }
                        if (pts > 0)
                            sb.AppendFormat("{0}({1}) ", c.Name, pts);
                    }
                }
                LoggerHelper.DebugFormat("{0} has now {1} victory points. {2}", p.Name, p.VictoryPoints, sb.ToString());
            }
        }
    }
}
