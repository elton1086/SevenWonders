using SevenWonder.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonder.Contracts
{
    public interface IPlayer
    {
        /// <summary>
        /// The wonder in play.
        /// </summary>
        IWonder Wonder { get; }
        /// <summary>
        /// All the cards the player has played.
        /// </summary>
        IList<IStructureCard> Cards { get; }
        /// <summary>
        /// Player name.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Coins the user owns
        /// </summary>
        int Coins { get; }
        /// <summary>
        /// Counts the total of cards of a certain type.
        /// </summary>
        /// <param name="type">The type of the card.</param>
        /// <returns></returns>
        int CardTypeCount(StructureType type);
        /// <summary>
        /// Sum of all conflict tokens.
        /// </summary>
        int ConflictTokenSum { get; }
        /// <summary>
        /// All conflict tokens hold by the player.
        /// </summary>
        IList<ConflictToken> ConflictTokens { get; }
        /// <summary>
        /// Victory points total for the player.
        /// </summary>
        int VictoryPoints { get; set; }
    }
}
