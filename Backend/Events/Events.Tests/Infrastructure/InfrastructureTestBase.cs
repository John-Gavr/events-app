using Events.Core.Interfaces;
using Events.Infrastructure.Data;
using Moq;

namespace Events.Tests.Infrastructure;

public class InfrastructureTestBase : IDisposable
{
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
    protected readonly AppDbContext _context;

    protected InfrastructureTestBase()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _context = AppDbContextFactory.Create();
        _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).Callback(() => _context.SaveChangesAsync());
    }
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
