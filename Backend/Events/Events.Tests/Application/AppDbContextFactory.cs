using Events.Infrastructure.Data;
using Events.Tests.Application.Tests.Users;
using Microsoft.EntityFrameworkCore;

namespace Events.Tests.Application;
public static class AppDbContextFactory
{
    private static AppDbContext _context;

    public static AppDbContext Create()
    {
        if (_context != null)
            return _context;

        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(dbContextOptions);

        if (!_context.Users.Any())
        {
            _context.Users.AddRange(UsersTestBase.Users);
            _context.SaveChanges();
        }

        return _context;
    }
}

