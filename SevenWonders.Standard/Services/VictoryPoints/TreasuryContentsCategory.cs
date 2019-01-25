using SevenWonders.Contracts;
using SevenWonders.Entities;
using System;
using System.Collections.Generic;

namespace SevenWonders.Services.VictoryPoints
{
    public class TreasuryContentsCategory : PointsCategory
    {
        private readonly ICoreLogger logger;

        public TreasuryContentsCategory(ICoreLogger logger)
        {
            this.logger = logger;
        }

        protected override void Compute<TPlayer>(IList<TPlayer> players)
        {
            logger.Debug("Starting to compute treasury victory points");
            foreach (var p in players)
            {
                p.VictoryPoints += (int)Math.Floor(p.Coins / 3.0);
                logger.Debug("{0} has now {1} victory points. Coins({2})", p.Name, p.VictoryPoints, p.Coins);
            }
        }
    }
}
