using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities.Events
{
    public class ClearTemporaryResourceEvent : IEvent
    {
        private TurnPlayer player;

        public ClearTemporaryResourceEvent(TurnPlayer player)
        {
            this.player = player;
        }

        public void Commit()
        {
            player.ResetData();
        }
    }
}
