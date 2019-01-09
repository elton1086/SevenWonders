using SevenWonders.BaseEntities;
using SevenWonders.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Extensions
{
    public static class TurnPlayerExtensions
    {
        public static void MoveSelectableCards(this IEnumerable<TurnPlayer> players, Age age)
        {
            switch (age)
            {
                case Age.I:
                case Age.III:
                    players.MoveCards();
                    break;
                case Age.II:
                    players.Reverse().MoveCards();
                    break;
                default:
                    break;
            }
        }

        private static void MoveCards(this IEnumerable<TurnPlayer> players)
        {
            var cardsToMove = players.Last().SelectableCards;
            foreach (var currentPlayer in players)
            {
                var nextCards = currentPlayer.SelectableCards;
                currentPlayer.SetSelectableCards(cardsToMove);
                cardsToMove = nextCards;
            }
        }

        public static void InitializeTurnData(this IList<TurnPlayer> players)
        {
            foreach (var p in players)
                p.InitializeTurnData();
        }
    }
}
