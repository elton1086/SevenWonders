using SevenWonder.Contracts;
using SevenWonder.Helper;
using System;
using System.Collections.Generic;

namespace SevenWonder.Services.VictoryPoints
{
    public class TreasuryContentsCategory : PointsCategory
    {
        protected override void Compute(IList<IPlayer> players)
        {
            LoggerHelper.DebugFormat("Starting to compute treasury victory points");
            foreach (var p in players)
            {
                p.VictoryPoints += (int)(p.Coins / 3);
                LoggerHelper.DebugFormat("{0} has now {1} victory points. Coins({2})", p.Name, p.VictoryPoints, p.Coins);
            }
        }
    }
}
