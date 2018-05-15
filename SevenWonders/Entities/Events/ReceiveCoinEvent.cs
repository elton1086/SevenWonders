using SevenWonder.Contracts;

namespace SevenWonder.Entities.Events
{
    public class ReceiveCoinEvent : IEvent
    {
        IGamePlayer player;
        int quantity;

        public ReceiveCoinEvent(IGamePlayer player, int quantity)
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
