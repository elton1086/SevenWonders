
namespace SevenWonder.Contracts
{
    public interface IUnitOfWork
    {
        void AddEvent(IEvent eventToAdd);
        void Commit();
    }
}
