using SevenWonders.Contracts;
using SevenWonders.Entities;
using System.Collections.Generic;

namespace SevenWonders.Services.VictoryPoints
{
    public class MilitaryConflictsCategory : PointsCategory
    {
        private readonly ICoreLogger logger;

        public MilitaryConflictsCategory(ICoreLogger logger)
        {
            this.logger = logger;
        }

        protected override void Compute<TPlayer>(IList<TPlayer> players)
        {
            logger.Debug("Starting to compute military conflict victory points");
            foreach (var p in players)
            {
                p.VictoryPoints += p.ConflictTokenSum;
                logger.Debug("{0} has now {1} victory points.", p.Name, p.VictoryPoints);
            }
        }
    }
}
