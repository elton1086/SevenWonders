using SevenWonder.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonder.Entities.Events
{
    public class BuildStageEvent : IEvent
    {
        IGamePlayer player;

        public BuildStageEvent(IGamePlayer player)
        {
            this.player = player;
        }

        public void Commit()
        {
            player.Wonder.BuildStage();
        }
    }
}
