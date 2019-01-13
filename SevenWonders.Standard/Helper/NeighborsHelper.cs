using SevenWonders.Entities;
using System.Collections.Generic;

namespace SevenWonders.Helper
{
    public static class NeighborsHelper
    {
        public const string RIGHTDIRECTION = "right", LEFTDIRECTION = "left";

        /// <summary>
        /// Get neighbors. Clarification: consider clockwise direction, so a player in position 1 has player in position 2 as left neighbor and last player position as right neighbor
        /// </summary>
        /// <param name="players"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static IDictionary<string, TPlayer> GetNeighbors<TPlayer>(this TPlayer current, IList<TPlayer> players) where TPlayer : Player
        {
            var result = new Dictionary<string, TPlayer>();
            var index = players.IndexOf(current);
            int right = 0, left = 0;

            if (index + 1 == players.Count)
                left = 0;
            else
                left = index + 1;

            if (index - 1 < 0)
                right = players.Count - 1;
            else
                right = index - 1;

            result.Add(RIGHTDIRECTION, players[right]);
            result.Add(LEFTDIRECTION, players[left]);
            return result;
        }
    }
}
