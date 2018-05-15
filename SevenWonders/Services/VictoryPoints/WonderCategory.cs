using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Helper;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonder.Services.VictoryPoints
{
    public class WonderCategory : PointsCategory
    {
        protected override void Compute(IList<IPlayer> players)
        {
            LoggerHelper.DebugFormat("Starting to compute wonder stage victory points");
            foreach (var p in players)
            {
                foreach (var e in p.Wonder.EffectsAvailable.Where(e => e.Type == EffectType.VictoryPoint))
                {
                    p.VictoryPoints += e.Quantity;
                }
                LoggerHelper.DebugFormat("{0} has now {1} victory points.", p.Name, p.VictoryPoints);
            }
        }
    }
}
