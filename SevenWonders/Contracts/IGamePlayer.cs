using SevenWonder.BaseEntities;
using System.Collections.Generic;

namespace SevenWonder.Contracts
{
    public interface IGamePlayer : IPlayer
    {
        /// <summary>
        /// Get all the resources available for this player.
        /// </summary>
        /// <param name="shareableOnly">Whether to select only resources that can be shared with other players or not.</param>
        /// <returns></returns>
        IList<ResourceType> GetResourcesAvailable(bool shareableOnly);
        ///// <summary>
        ///// Get the availability of the resources that can be choosable. Return the resources that are not available.
        ///// </summary>
        ///// <param name="resources">The list of resources to find in choosable list.</param>
        ///// <param name="shareableOnly">Whether to select only resources that can be shared with other players or not.</param>
        ///// <returns></returns>
        //IList<ResourceType> ChoosableResourceAvailability(IList<ResourceType> resources, bool shareableOnly);
        /// <summary>
        /// Get a list of choosable resources.
        /// </summary>
        /// <param name="shareableOnly">Whether to select only resources that can be shared with other players or not.</param>
        /// <returns></returns>
        IList<IList<ResourceType>> GetChoosableResources(bool shareableOnly);
        /// <summary>
        /// Check if the player can buy with discount from neighbor.
        /// </summary>
        /// <param name="direction">Right of left neighbor.</param>
        /// <param name="type">Type of the discount.</param>
        /// <returns></returns>
        bool HasDiscount(PlayerDirection direction, TradeDiscountType type);
        /// <summary>
        /// Get all the effects, except resources.
        /// </summary>
        /// <returns></returns>
        IList<IEffect> GetNonResourceEffects();
        /// <summary>
        /// Add coins to its treasury
        /// </summary>
        /// <param name="quantity">number of coins</param>
        void ReceiveCoin(int quantity);
        /// <summary>
        /// Pay coins from its trasury
        /// </summary>
        /// <param name="quantity">number of coins</param>
        void PayCoin(int quantity);
        /// <summary>
        /// Define wonder to be played
        /// </summary>
        /// <param name="wonder"></param>
        void SetWonder(IWonder wonder);
        /// <summary>
        /// Check the availability for a list of resources. Return the resources that are not available.
        /// </summary>
        /// <param name="neededResources">Resources the player is in need of.</param>
        /// <param name="shareableOnly">Whether to select only resources that can be shared with other players or not.</param>
        /// <returns></returns>
        IList<ResourceType> CheckResourceAvailability(IList<ResourceType> neededResources, bool shareableOnly);
        /// <summary>
        /// The player's current military power.
        /// </summary>
        /// <returns></returns>
        int GetMilitaryPower();
        /// <summary>
        /// Check if player already has the card.
        /// </summary>
        /// <param name="card">The card to check.</param>
        /// <returns></returns>
        bool HasCard(IStructureCard card);
    }
}
