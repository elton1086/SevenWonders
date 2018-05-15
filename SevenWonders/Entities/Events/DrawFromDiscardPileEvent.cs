using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Entities.Events
{
    public class DiscardCardEvent : IEvent
    {
        IList<IStructureCard> discardedCards;
        IStructureCard cardToAdd;

        public DiscardCardEvent(IStructureCard cardToAdd, IList<IStructureCard> discardedCards)
        {
            this.cardToAdd = cardToAdd;
            this.discardedCards = discardedCards;
        }

        public void Commit()
        {
            discardedCards.Add(cardToAdd);
        }
    }
}
