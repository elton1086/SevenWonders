using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Services.Contracts
{
    public interface ITradeManager
    {
        /// <summary>
        /// Setup coins for each player.
        /// </summary>
        /// <param name="players">List of players playing the game.</param>
        void SetupCoinsFromBank(IList<IGamePlayer> players);
        /// <summary>
        /// Borrow a certain resource from a neighbor.
        /// </summary>
        /// <param name="player">Player in need of the resource.</param>
        /// <param name="rightNeighbor">The right neighbor of the player in need of the resource.</param>
        /// <param name="leftNeighbor">The left neighbor of the player in need of the resource.</param>
        /// <param name="data">Data containing the resource type, quantity and neighbor preference to borrow.</param>
        /// <param name="allowAutomaticChoice">Allows to automatic select the neighbor to borrow from.</param>
        /// <returns></returns>
        bool BorrowResources(ITurnPlayer player, IGamePlayer rightNeighbor, IGamePlayer leftNeighbor, IList<BorrowResourceData> data, bool allowAutomaticChoice);
        /// <summary>
        /// Resolve all military conflicts among players.
        /// </summary>
        /// <param name="players">The players participating in the game.</param>
        /// <param name="age">The age that was played.</param>
        void ResolveMilitaryConflicts(IList<IGamePlayer> players, Age age);
    }

    public class BorrowResourceData
    {
        public PlayerDirection ChosenNeighbor;
        public ResourceType ResourceType;
    }
}
