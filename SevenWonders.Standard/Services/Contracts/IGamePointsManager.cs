using SevenWonders.Entities;
using System.Collections.Generic;

namespace SevenWonders.Services.Contracts
{
    public interface IGamePointsManager
    {
        void ComputeVictoryPoints(IList<Player> players);
    }
}
