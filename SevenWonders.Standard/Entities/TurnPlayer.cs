using SevenWonders.BaseEntities;
using SevenWonders.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Entities
{
    public class TurnPlayer
    {
        private StructureCard selectedCard;

        public TurnPlayer(GamePlayer player)
        {
            GamePlayer = player;
            ResetData();
        }

        public GamePlayer GamePlayer { get; private set; }
        /// <summary>
        /// Gets all the temporary resources the player has.
        /// </summary>
        public List<ResourceType> TemporaryResources { get; } = new List<ResourceType>();
        /// <summary>
        /// Cards available to the player for this turn.
        /// </summary>
        public List<StructureCard> SelectableCards { get; } = new List<StructureCard>();
        /// <summary>
        /// Selected card.
        /// </summary>
        public StructureCard SelectedCard
        {
            get { return this.selectedCard; }
            set
            {
                if (value == null)
                    selectedCard = null;
                else
                {
                    //allows to select card that is not part of the selectable cards so when checking rewards can use this same property
                    SelectableCards.Remove(value);
                    selectedCard = value;
                }
            }
        }
        /// <summary>
        /// Action to be executed for the turn.
        /// </summary>
        public TurnAction ChosenAction { get; set; }
        /// <summary>
        /// Special case to be used.
        /// </summary>
        public SpecialCaseType SpecialCaseToUse { get; set; }
        /// <summary>
        /// Resources to borrow from neighbors.
        /// </summary>
        public IList<BorrowResourceData> ResourcesToBorrow { get; } = new List<BorrowResourceData>();
        /// <summary>
        /// Action actually executed after turn.
        /// </summary>
        public TurnAction ExecutedAction { get; set; }
        /// <summary>
        /// Additional info used by rewards.
        /// </summary>
        public object AdditionalInfo { get; set; }
        /// <summary>
        /// Coins left for this turn.
        /// </summary>
        public int CoinsLeft { get; set; }

        public bool CanPlayCard
        {
            get
            {
                if (ChosenAction != TurnAction.BuyCard)
                    return true;
                return !GamePlayer.Cards.Any(c => c.Name == SelectedCard.Name);
            }
        }

        public IList<ResourceType> GetResourcesAvailable(bool shareableOnly)
        {
            var resourcesAvailable = GamePlayer.GetResourcesAvailable(shareableOnly).ToList();
            if (shareableOnly)
                return resourcesAvailable;
            resourcesAvailable.AddRange(TemporaryResources);
            return resourcesAvailable;
        }

        /// <summary>
        /// Add temporary resources to be given to the player when turn ends.
        /// </summary>
        /// <param name="resources"></param>
        public void AddTemporaryResources(IList<ResourceType> resources)
        {
            TemporaryResources.AddRange(resources);
        }

        /// <summary>
        /// Add selectable cards for the turn.
        /// </summary>
        /// <param name="cards"></param>
        public void SetSelectableCards(IList<StructureCard> cards)
        {
            if (cards == null)
                throw new NullReferenceException("cards cannot be null");
            SelectableCards.Clear();
            SelectableCards.AddRange(cards);
        }

        /// <summary>
        /// Initialize all necessary data for the turn and clear temporary resources.
        /// </summary>
        public void ResetData()
        {
            SelectedCard = null;
            ChosenAction = TurnAction.SellCard;
            SpecialCaseToUse = SpecialCaseType.None;
            ResourcesToBorrow.Clear();
            AdditionalInfo = null;
            CoinsLeft = GamePlayer.Coins;
            TemporaryResources.Clear();
        }
    }
}
