using SevenWonder.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonder.Services.VictoryPoints
{
    public abstract class PointsCategory
    {
        private PointsCategory successor;

        public void SetSuccessor(PointsCategory successor)
        {
            this.successor = successor;
        }        

        public void ComputePoints(IList<IPlayer> players)
        {
            Compute(players);
            if (this.successor != null)
                this.successor.ComputePoints(players);
        }

        protected abstract void Compute(IList<IPlayer> players);
    }
}
