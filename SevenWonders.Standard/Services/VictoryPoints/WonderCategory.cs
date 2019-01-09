using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Services.VictoryPoints
{
    public class WonderCategory : PointsCategory
    {
        private readonly ICoreLogger logger;

        public WonderCategory(ICoreLogger logger)
        {
            this.logger = logger;
        }

        protected override void Compute(IList<Player> players)
        {
            logger.Debug("Starting to compute wonder stage victory points");
            foreach (var p in players)
            {
                foreach (var e in p.Wonder.EffectsAvailable.Where(e => e.Type == EffectType.VictoryPoint))
                {
                    p.VictoryPoints += e.Quantity;
                }
                logger.Debug("{0} has now {1} victory points.", p.Name, p.VictoryPoints);
            }
        }
    }
}
