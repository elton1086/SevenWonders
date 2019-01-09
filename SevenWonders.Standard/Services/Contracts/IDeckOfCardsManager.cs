using SevenWonders.Contracts;
using SevenWonders.Entities;
using System.Collections.Generic;

namespace SevenWonders.Services.Contracts
{
    public interface IDeckOfCardsManager
    {
        /// <summary>
        /// Gets all the cards available for the informed number of players (includes guilds) and shuffle them.
        /// </summary>
        /// <param name="numberOfPlayers">Number of players to play the game.</param>
        /// <returns></returns>
        IList<StructureCard> GetShuffledDeck(int numberOfPlayers);
        /// <summary>
        /// Get all the wonder cards.
        /// </summary>
        /// <returns></returns>        
        IList<WonderCard> GetShuffledWonderCards();
    }
}
