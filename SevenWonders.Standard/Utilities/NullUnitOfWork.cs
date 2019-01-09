using SevenWonders.Contracts;
using System;

namespace SevenWonders.Utilities
{
    public class NullUnitOfWork : IUnitOfWork
    {
        public void AddEvent(IEvent eventToAdd)
        {
            return;
        }

        public void Commit()
        {
            return;
        }
    }
}
