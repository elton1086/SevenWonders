using SevenWonder.Contracts;
using SevenWonder.Helper;
using SevenWonder.Services.Contracts;
using SevenWonder.Services.VictoryPoints;
using System.Collections.Generic;

namespace SevenWonder.Services
{
    public class GamePointsManager : IGamePointsManager
    {
        PointsCategory pointsCategory;

        public GamePointsManager()
        {
            pointsCategory = new TreasuryContentsCategory();
            var military = new MilitaryConflictsCategory();
            var wonder = new WonderCategory();
            var civilian = new CivilianStructuresCategory();
            var scientific = new ScientificStructuresCategory();
            var commercial = new CommercialStructuresCategory();
            var guilds = new GuildsCategory();

            pointsCategory.SetSuccessor(military);
            military.SetSuccessor(wonder);
            wonder.SetSuccessor(civilian);
            civilian.SetSuccessor(scientific);
            scientific.SetSuccessor(commercial);
            commercial.SetSuccessor(guilds);
        }

        public void ComputeVictoryPoints(IList<IPlayer> players)
        {
            LoggerHelper.Info("Starting to compute players victory points");
            pointsCategory.ComputePoints(players);
        }
    }
}
