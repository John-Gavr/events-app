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
            new EventParticipant { Id = 6, FirstName = "Doe", LastName = "Smith", UserId = Guid.Parse("E435148A-5CD8-4513-BA51-2C8B4D091684"), EventId = 2 },
            new EventParticipant { Id = 27, UserId = Guid.Parse("f9bbe22b-8d84-4365-9aa3-605ec45dc75d") },
            new EventParticipant { Id = 28, UserId = Guid.NewGuid() }
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
            },
            new Event { Id = 23, EventDateTime = DateTime.Now.AddDays(1), Participants = [] },
            new Event { Id = 24, EventDateTime = DateTime.Now.AddDays(2), Participants = [] }
            );

        context.SaveChanges();

        var event1 = context.Events.FirstOrDefault(e => e.Id == 23);
        var participant1 = context.EventParticipants.FirstOrDefaultAsync(p => p.Id == 27);
        event1!.Participants.Add(participant1.Result!);

        var event2 = context.Events.FirstOrDefault(e => e.Id == 24);
        var participant2 = context.EventParticipants.FirstOrDefaultAsync(p => p.Id == 28);
        event2!.Participants.Add(participant2.Result!);

        context.SaveChanges();
        return context;
    }
}
