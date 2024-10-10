using Events.Infrastructure.Data;

namespace Events.Tests.Infrastructure;
public class InfrastructureTestBase : IDisposable
{
    protected readonly AppDbContext _context;

    protected InfrastructureTestBase()
    {
        _context = AppDbContextFactory.Create();
    }
    public void Dispose()
    {
        _context.Database.EnsureDeleted(); 
        _context.Dispose();
    }

    protected static int PageNumber = 1;
    protected static int PageSize = 2;
}
