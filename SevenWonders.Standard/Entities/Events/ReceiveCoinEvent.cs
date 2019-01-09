using SevenWonders.Contracts;

namespace SevenWonders.Entities.Events
{
    public class ReceiveCoinEvent : IEvent
    {
        GamePlayer player;
        int quantity;

        public ReceiveCoinEvent(GamePlayer player, int quantity)
        {
            this.player = player;
            this.quantity = quantity;
        }

        public void Commit()
        {
            player.ReceiveCoin(quantity);
        }
    }
}
