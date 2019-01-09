using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities.Events
{
    public class DrawFromDiscardPileEvent : IEvent
    {
        IList<StructureCard> discardedCards;
        StructureCard cardToRemove;

        public DrawFromDiscardPileEvent(StructureCard cardToRemove, IList<StructureCard> discardedCards)
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
