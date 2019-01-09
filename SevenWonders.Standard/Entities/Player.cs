using SevenWonders.BaseEntities;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Entities
{
    public abstract class Player
    {
        public Player(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The wonder in play.
        /// </summary>
        public BaseWonder Wonder { get; protected set; } = null;
        /// <summary>
        /// All the cards the player has played.
        /// </summary>
        public IList<StructureCard> Cards { get; } = new List<StructureCard>();
        /// <summary>
        /// Player name.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Coins the user owns
        /// </summary>
        public int Coins { get; protected set; }
        /// <summary>
        /// All conflict tokens hold by the player.
        /// </summary>
        public IList<ConflictToken> ConflictTokens { get; } = new List<ConflictToken>();
        /// <summary>
        /// Victory points total for the player.
        /// </summary>
        public int VictoryPoints { get; set; }
        /// <summary>
        /// Sum of all conflict tokens.
        /// </summary>
        public int ConflictTokenSum
        {
            get
            {
                int result = 0;
                foreach (var t in ConflictTokens)
                    result += (int)t;
                return result;
            }
        }
        /// <summary>
        /// Counts the total of cards of a certain type.
        /// </summary>
        /// <param name="type">The type of the card.</param>
        /// <returns></returns>
        public int CardTypeCount(StructureType type)
        {
            return Cards.Count(c => c.Type == type);
        }
    }
}
