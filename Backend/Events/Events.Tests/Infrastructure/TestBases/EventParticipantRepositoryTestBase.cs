using Events.Core.Entities;

namespace Events.Tests.Infrastructure.TestBases;
public class EventParticipantRepositoryTestBase : InfrastructureTestBase 
{
    protected static int EventIdToRegisterParticipant = 2;
    protected static int EventIdToUnregisterUser = 2;
    protected static int EventIdToGetParticipants = 1;
    protected static string UserIdToGetParticipant = "e435148a-5cd8-4513-ba51-2c8b4d091684";
    protected static string UserIdToUnregister = "e435148a-5cd8-4513-ba51-2c8b4d091684";
    protected static EventParticipant ParticipantEntityToRegister = new EventParticipant()
    {
        FirstName = "John",
        LastName = "Brown",
        DateOfBirth = DateTime.Parse("2001-04-22"),
        RegistrationDate = DateTime.Now,
        Email = "john@gmail.com",
        UserId = Guid.NewGuid(),
        EventId = EventIdToRegisterParticipant
    };
}
