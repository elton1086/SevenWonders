using SevenWonder.Contracts;

namespace SevenWonder.Entities.Events
{
    public class AddCardEvent : IEvent
    {
        private IGamePlayer player;
        private IStructureCard card;

        public AddCardEvent(IGamePlayer player, IStructureCard card)
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
