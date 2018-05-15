using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonder.Services.Contracts
{
    public interface IDeckOfCardsManager
    {
        /// <summary>
        /// Gets all the cards available for the informed number of players (includes guilds) and shuffle them.
        /// </summary>
        /// <param name="numberOfPlayers">Number of players to play the game.</param>
        /// <returns></returns>
        IList<IStructureCard> GetShuffledDeck(int numberOfPlayers);
        /// <summary>
        /// Get all the wonder cards.
        /// </summary>
        /// <returns></returns>        
        IList<WonderCard> GetShuffledWonderCards();
    }
}
