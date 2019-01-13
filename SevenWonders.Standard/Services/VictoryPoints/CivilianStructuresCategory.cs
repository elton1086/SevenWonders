using SevenWonders.Contracts;
using SevenWonders.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenWonders.Services.VictoryPoints
{
    public class CivilianStructuresCategory : PointsCategory
    {
        private readonly ICoreLogger logger;

        public CivilianStructuresCategory(ICoreLogger logger)
        {
            this.logger = logger;
        }

        protected override void Compute<TPlayer>(IList<TPlayer> players)
        {
            logger.Debug("Starting to compute civilian structure victory points");
            foreach (var p in players)
            {
                var sb = new StringBuilder();
                foreach (var c in p.Cards.Where(c => c.Type == BaseEntities.StructureType.Civilian))
                {
                    foreach (var e in c.Production)
                    {
                        if (e.Type != BaseEntities.EffectType.VictoryPoint)
                            return;
                        sb.AppendFormat("{0}({1}) ", c.Name, e.Quantity);
                        p.VictoryPoints += e.Quantity;
                    }
                }
                logger.Debug("{0} has now {1} victory points. {2}", p.Name, p.VictoryPoints, sb.ToString());
            }
        }
    }
}
