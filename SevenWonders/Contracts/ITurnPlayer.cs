using SevenWonder.BaseEntities;
using SevenWonder.Services.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Contracts
{
    public interface ITurnPlayer : IGamePlayer
    {
        /// <summary>
        /// Gets all the temporary resources the player has.
        /// </summary>
        IList<ResourceType> TemporaryResources { get; }
        /// <summary>
        /// Cards available to the player for this turn.
        /// </summary>
        IList<IStructureCard> SelectableCards { get; }
        /// <summary>
        /// Selected card.
        /// </summary>
        IStructureCard SelectedCard { get; set; }
        /// <summary>
        /// Action to be executed for the turn.
        /// </summary>
        TurnAction ChosenAction { get; set; }
        /// <summary>
        /// Action actually executed after turn.
        /// </summary>
        TurnAction ExecutedAction { get; set; }
        /// <summary>
        /// Special case to be used.
        /// </summary>
        SpecialCaseType SpecialCaseToUse { get; set; }
        /// <summary>
        /// Coins left for this turn.
        /// </summary>
        int CoinsLeft { get; set; }
        /// <summary>
        /// Resources to borrow from neighbors.
        /// </summary>
        IList<BorrowResourceData> ResourcesToBorrow { get; }
        /// <summary>
        /// Additional info used by rewards.
        /// </summary>
        object AdditionalInfo { get; set; }
        /// <summary>
        /// Add temporary resources to be given to the player when turn ends.
        /// </summary>
        /// <param name="resources"></param>
        void AddTemporaryResources(IList<ResourceType> resources);
        /// <summary>
        /// Initialize all necessary data for the turn and clear temporary resources.
        /// </summary>
        void InitializeTurnData();
        /// <summary>
        /// Add selectable cards for the turn.
        /// </summary>
        /// <param name="cards"></param>
        void SetSelectableCards(IList<IStructureCard> cards);
        /// <summary>
        /// Clone this player.
        /// </summary>
        /// <returns></returns>
        ITurnPlayer Clone();
    }
}
