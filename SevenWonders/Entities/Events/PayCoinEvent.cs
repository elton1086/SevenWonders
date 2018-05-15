using SevenWonder.Contracts;

namespace SevenWonder.Entities.Events
{
    public class PayCoinEvent : IEvent
    {
        IGamePlayer player;
        int quantity;

        public PayCoinEvent(IGamePlayer player, int quantity)
        {
            this.player = player;
            this.quantity = quantity;
        }

        public void Commit()
        {
            player.PayCoin(quantity);
        }
    }
}
