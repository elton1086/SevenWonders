using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities.Events
{
    public class DiscardCardEvent : IEvent
    {
        IList<StructureCard> discardedCards;
        StructureCard cardToAdd;

        public DiscardCardEvent(StructureCard cardToAdd, IList<StructureCard> discardedCards)
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
