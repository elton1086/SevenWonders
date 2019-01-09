using SevenWonders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Entities.Events
{
    public class BuildStageEvent : IEvent
    {
        GamePlayer player;

        public BuildStageEvent(GamePlayer player)
        {
            this.player = player;
        }

        public void Commit()
        {
            player.Wonder.BuildStage();
        }
    }
}
