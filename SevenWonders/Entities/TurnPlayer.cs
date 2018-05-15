using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenWonder.Services.Contracts;

namespace SevenWonder.Entities
{
    public class TurnPlayer : Player, ITurnPlayer
    {
        private List<IStructureCard> selectableCards = new List<IStructureCard>();
        private IStructureCard selectedCard;
        private SpecialCaseType specialCaseToUse;
        private List<ResourceType> tempResources = new List<ResourceType>();
        private IList<BorrowResourceData> resourcesToBorrow = new List<BorrowResourceData>();

        public TurnPlayer(string name) : base(name) { }

        public override IList<ResourceType> GetResourcesAvailable(bool shareableOnly)
        {
            var resourcesAvailable = base.GetResourcesAvailable(shareableOnly).ToList();
            if (shareableOnly)
                return resourcesAvailable;
            resourcesAvailable.AddRange(this.tempResources);
            return resourcesAvailable;
        }

        public IList<ResourceType> TemporaryResources
        {
            get { return tempResources; }
        }

        public IList<IStructureCard> SelectableCards
        {
            get { return selectableCards; }
        }

        public IStructureCard SelectedCard
        {
            get { return this.selectedCard; }
            set
            {
                if (value == null)
                    this.selectedCard = null;
                else
                {
                    //allows to select card that is not part of the selectable cards so when checking rewards can use this same property
                    this.selectableCards.Remove(value);
                    this.selectedCard = value;
                }
            }
        }

        public TurnAction ChosenAction { get; set; }

        public SpecialCaseType SpecialCaseToUse
        {
            get { return this.specialCaseToUse; }
            set { this.specialCaseToUse = value; }
        }

        public IList<BorrowResourceData> ResourcesToBorrow
        {
            get { return this.resourcesToBorrow; }
        }

        public TurnAction ExecutedAction { get; set; }

        public object AdditionalInfo { get; set; }

        public int CoinsLeft { get; set; }

        public void AddTemporaryResources(IList<ResourceType> resources)
        {
            tempResources.AddRange(resources);
        }        

        public void SetSelectableCards(IList<IStructureCard> cards)
        {
            if (cards == null)
                throw new NullReferenceException("cards cannot be null");
            this.selectableCards = new List<IStructureCard>(cards);
        }

        public void InitializeTurnData()
        {
            this.SelectedCard = null;
            this.ChosenAction = TurnAction.SellCard;
            this.specialCaseToUse = SpecialCaseType.None;
            this.resourcesToBorrow.Clear();
            this.AdditionalInfo = null;
            this.CoinsLeft = this.Coins;
            this.tempResources.Clear();
        }

        public ITurnPlayer Clone()
        {
            var player = new TurnPlayer(this.Name);
            player.SetWonder(this.Wonder);
            player.ReceiveCoin(this.CoinsLeft);
            ((List<IStructureCard>)player.Cards).AddRange(this.Cards);
            ((List<IStructureCard>)player.SelectableCards).AddRange(this.SelectableCards);
            return player;
        }
    }
}
