using Events.Infrastructure.Data;
using Events.Tests.Application.TestBases;
using Microsoft.EntityFrameworkCore;

namespace Events.Tests.Application;
public static class AppDbContextFactory
{
    public static AppDbContext Create()
    {
        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase("TestDatabase")
        .Options;
        var context = new AppDbContext(dbContextOptions);

        if(context.Users.Count() == 0)
            context.Users.AddRange(UserDataServiceTestBase.Users);

        context.SaveChanges();

        return context;
    }
}
