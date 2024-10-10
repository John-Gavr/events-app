using Events.Core.Entities;

namespace Events.Tests.Infrastructure.TestBases;
public class EventRepositoryTestBase : InfrastructureTestBase
{
    protected static int EventIdToGetWhenExist = 1;
    protected static int EventIdToGetWhenNotExist = 999;
    protected static int EventIdForDelete = 2;
    protected static DateTime DateCriteria = DateTime.Parse("2024-10-10");
    protected static string LocationCriteria = "Location of the second event";
    protected static int EventIdForUpdateImage = 1;
    protected static byte[] ImageBytes = [1, 2, 3, 4];
    protected static string UserIdToGetRelatedEventsOrCount = "f9bbe22b-8d84-4365-9aa3-605ec45dc75d";
    protected static Event NewEventToAdd = new Event
    {
        Name = "New Event",
        Description = "Test description",
        EventDateTime = DateTime.Parse("2024-10-10"),
        Location = "Test Location",
        MaxParticipants = 100
    };
    protected static Event UpdatedValuesForEvent = new Event
    {
        Name = "Updated Event",
        EventDateTime = DateTime.Parse("2025-01-25"),
        Location = "Updated Location",
        MaxParticipants = 150,
        Category = "Updated Category",
        Description = "Updated Description"
    }; 
}

