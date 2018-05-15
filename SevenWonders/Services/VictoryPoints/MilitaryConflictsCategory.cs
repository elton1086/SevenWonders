using SevenWonder.Contracts;
using SevenWonder.Helper;
using System.Collections.Generic;

namespace SevenWonder.Services.VictoryPoints
{
    public class MilitaryConflictsCategory : PointsCategory
    {
        protected override void Compute(IList<IPlayer> players)
        {
            LoggerHelper.DebugFormat("Starting to compute military conflict victory points");
            foreach (var p in players)
            {
                p.VictoryPoints += p.ConflictTokenSum;
                LoggerHelper.DebugFormat("{0} has now {1} victory points.", p.Name, p.VictoryPoints);
            }
        }
    }
}
