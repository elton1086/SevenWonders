using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Entities.Events
{
    public class DrawFromDiscardPileEvent : IEvent
    {
        IList<IStructureCard> discardedCards;
        IStructureCard cardToRemove;

        public DrawFromDiscardPileEvent(IStructureCard cardToRemove, IList<IStructureCard> discardedCards)
        {
            this.cardToRemove = cardToRemove;
            this.discardedCards = discardedCards;
        }

        public void Commit()
        {
            discardedCards.Remove(cardToRemove);
        }
    }
}
