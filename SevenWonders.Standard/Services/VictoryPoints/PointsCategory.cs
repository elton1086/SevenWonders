using SevenWonders.Entities;
using System.Collections.Generic;

namespace SevenWonders.Services.VictoryPoints
{
    public abstract class PointsCategory
    {
        private PointsCategory successor;

        public void SetSuccessor(PointsCategory successor)
        {
            this.successor = successor;
        }        

        public void ComputePoints(IList<Player> players)
        {
            Compute(players);
            if (this.successor != null)
                this.successor.ComputePoints(players);
        }

        protected abstract void Compute(IList<Player> players);
    }
}
