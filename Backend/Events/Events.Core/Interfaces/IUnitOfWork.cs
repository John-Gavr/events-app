
namespace Events.Core.Interfaces;

public interface IUnitOfWork
{
    Task<int> CompleteAsync();
}
