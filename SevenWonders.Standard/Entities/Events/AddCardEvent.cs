using SevenWonders.Contracts;

namespace SevenWonders.Entities.Events
{
    public class AddCardEvent : IEvent
    {
        private GamePlayer player;
        private StructureCard card;

        public AddCardEvent(GamePlayer player, StructureCard card)
        {
            this.player = player;
            this.card = card;
        }

        public void Commit()
        {
            player.Cards.Add(card);
        }

        public void Rollback()
        {
            player.Cards.Remove(card);
        }
    }
}
