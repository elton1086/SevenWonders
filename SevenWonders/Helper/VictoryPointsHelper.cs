using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonder.Helper
{
    public static class VictoryPointsHelper
    {
        public static int GetVictoryPointsByWonderStage(IPlayer player, int quantity, PlayerDirection direction = PlayerDirection.Myself, IPlayer rightPlayer = null, IPlayer leftPlayer = null)
        {
            var total = 0;
            if (direction.HasFlag(PlayerDirection.Myself))
                total += player.Wonder.StagesBuilt * quantity;
            if (direction.HasFlag(PlayerDirection.ToTheRight))
                total += rightPlayer.Wonder.StagesBuilt * quantity;
            if (direction.HasFlag(PlayerDirection.ToTheLeft))
                total += leftPlayer.Wonder.StagesBuilt * quantity;
            return total;
        }

        public static int GetVictoryPointsByStructureType(IPlayer player, int quantity, StructureType structureType, PlayerDirection direction = PlayerDirection.Myself, IPlayer rightPlayer = null, IPlayer leftPlayer = null)
        {
            var total = 0;
            if(direction.HasFlag(PlayerDirection.Myself))
                total += player.Cards.Count(c => c.Type == structureType) * quantity;
            if (direction.HasFlag(PlayerDirection.ToTheRight))
                total += rightPlayer.Cards.Count(c => c.Type == structureType) * quantity;
            if (direction.HasFlag(PlayerDirection.ToTheLeft))
                total += leftPlayer.Cards.Count(c => c.Type == structureType) * quantity;
            return total;
        }

        public static int GetVictoryPointsByConflictDefeat(IPlayer player, int quantity, StructureType structureType, PlayerDirection direction = PlayerDirection.Myself, IPlayer rightPlayer = null, IPlayer leftPlayer = null)
        {
            var total = 0;
            if (direction.HasFlag(PlayerDirection.Myself))
                total += player.ConflictTokens.Count(t => t == ConflictToken.Defeat) * quantity;
            if (direction.HasFlag(PlayerDirection.ToTheRight))
                total += rightPlayer.ConflictTokens.Count(t => t == ConflictToken.Defeat) * quantity;
            if (direction.HasFlag(PlayerDirection.ToTheLeft))
                total += leftPlayer.ConflictTokens.Count(t => t == ConflictToken.Defeat) * quantity;
            return total;
        }
    }
}
