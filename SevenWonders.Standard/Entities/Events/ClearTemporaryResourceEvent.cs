using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities.Events
{
    public class ClearTemporaryResourceEvent : IEvent
    {
        private TurnPlayer player;
        private IList<ResourceType> temporary;

        public ClearTemporaryResourceEvent(TurnPlayer player)
        {
            this.player = player;
        }

        public void Commit()
        {
            temporary = player.TemporaryResources;
            player.ResetData();
        }
    }
}
