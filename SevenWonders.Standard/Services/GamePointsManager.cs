using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Services.Contracts;
using SevenWonders.Services.VictoryPoints;
using System.Collections.Generic;

namespace SevenWonders.Services
{
    public class GamePointsManager : IGamePointsManager
    {
        private readonly PointsCategory pointsCategory;
        private readonly ICoreLogger logger;

        public GamePointsManager(PointsCategory pointsCategory, ICoreLogger logger)
        {
            this.logger = logger;
            this.pointsCategory = pointsCategory;
            //pointsCategory = new TreasuryContentsCategory();
            //var military = new MilitaryConflictsCategory();
            //var wonder = new WonderCategory();
            //var civilian = new CivilianStructuresCategory();
            //var scientific = new ScientificStructuresCategory();
            //var commercial = new CommercialStructuresCategory();
            //var guilds = new GuildsCategory();

            //pointsCategory.SetSuccessor(military);
            //military.SetSuccessor(wonder);
            //wonder.SetSuccessor(civilian);
            //civilian.SetSuccessor(scientific);
            //scientific.SetSuccessor(commercial);
            //commercial.SetSuccessor(guilds);
        }

        public void ComputeVictoryPoints(IList<Player> players)
        {
            logger.Info("Starting to compute players victory points");
            pointsCategory.ComputePoints(players);
        }
    }
}
