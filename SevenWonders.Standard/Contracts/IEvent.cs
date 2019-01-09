using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Contracts
{
    /// <summary>
    /// Stores and event that should be commited at the end of the transaction.
    /// </summary>
    public interface IEvent
    {
        void Commit();
    }
}
