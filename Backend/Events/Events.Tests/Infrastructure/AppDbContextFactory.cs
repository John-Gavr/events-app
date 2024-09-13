using Events.Core.Entities;
using Events.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Events.Tests.Infrastructure;

public static class AppDbContextFactory
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new AppDbContext(options);

        context.AddRange(
            new EventParticipant { Id = 1, FirstName = "John", LastName = "Doe", UserId = Guid.NewGuid(), EventId = 1 },
            new EventParticipant { Id = 2, FirstName = "Jane", LastName = "Doe", UserId = Guid.NewGuid(), EventId = 1 },
            new EventParticipant { Id = 3, FirstName = "Paul", LastName = "Smith", UserId = Guid.NewGuid(), EventId = 1 },
            new EventParticipant { Id = 4, FirstName = "Anna", LastName = "Taylor", UserId = Guid.NewGuid() , EventId = 1 },
            new EventParticipant { Id = 5, FirstName = "Bob", LastName = "Lee", UserId = Guid.NewGuid() , EventId = 1 },
            new EventParticipant { Id = 6, FirstName = "Doe", LastName = "Smith", UserId = Guid.Parse("E435148A-5CD8-4513-BA51-2C8B4D091684"), EventId = 2 }
        );

        context.AddRange(
            new Event
            {
                Id = 1,
                MaxParticipants = 5,
            },
            new Event
            {
                Id = 2,
                MaxParticipants = 5,
                Location = "Location of the second event",
                EventDateTime = DateTime.Parse("2024-10-10")
            });
        context.SaveChanges();
        return context;
    }
}
