using SevenWonder.Contracts;
using System.Collections.Generic;

namespace SevenWonder.Utilities
{
    public class UnitOfWork : IUnitOfWork
    {
        private List<IEvent> events;

        public UnitOfWork()
        {
            this.events = new List<IEvent>();
        }

        public void AddEvent(IEvent eventToAdd)
        {
            this.events.Add(eventToAdd);
        }

        public void Commit()
        {
            foreach (var e in events)
                e.Commit();
        }
    }
}
