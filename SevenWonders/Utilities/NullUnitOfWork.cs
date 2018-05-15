using SevenWonder.Contracts;
using System;

namespace SevenWonder.Utilities
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
