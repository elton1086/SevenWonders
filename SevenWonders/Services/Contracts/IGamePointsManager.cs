using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Services.Contracts
{
    public interface IGamePointsManager
    {
        void ComputeVictoryPoints(IList<IPlayer> players);
    }
}
