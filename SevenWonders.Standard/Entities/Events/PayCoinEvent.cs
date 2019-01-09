using SevenWonders.Contracts;

namespace SevenWonders.Entities.Events
{
    public class PayCoinEvent : IEvent
    {
        GamePlayer player;
        int quantity;

        public PayCoinEvent(GamePlayer player, int quantity)
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
