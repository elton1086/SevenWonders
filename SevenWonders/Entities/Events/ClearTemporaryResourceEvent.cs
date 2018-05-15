using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Entities.Events
{
    public class ClearTemporaryResourceEvent : IEvent
    {
        private ITurnPlayer player;
        private IList<ResourceType> temporary;

        public ClearTemporaryResourceEvent(ITurnPlayer player)
        {
            this.player = player;
        }

        public void Commit()
        {
            temporary = player.TemporaryResources;
            player.InitializeTurnData();
        }
    }
}
