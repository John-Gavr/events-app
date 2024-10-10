using Events.Application.DTOs.Participants.Requests.GetParticipantById;
using Events.Application.DTOs.Participants.Requests.GetParticipantsByEventId;
using Events.Application.DTOs.Participants.Requests.RegisterParticipant;
using Events.Application.DTOs.Participants.Requests.UnregisterParticipant;
using Events.Core.Entities;

namespace Events.Tests.Application.TestBases;
public class EventParticipantServiceTestBase : ApplicationTestBase
{
    protected static readonly RegisterParticipantRequest RegisterParticipantRequest = new RegisterParticipantRequest
    {
        EventId = 1
    };
    protected static readonly EventParticipant TestParticipant = new EventParticipant
    {
        EventId = RegisterParticipantRequest.EventId,
        UserId = Guid.Parse(UserId)
    };
    protected static readonly GetParticipantsByEventIdRequest GetParticipantsByEventIdRequest =
        new GetParticipantsByEventIdRequest
        {
            EventId = 1,
            PageNumber = 1,
            PageSize = 10
        };
    protected static readonly GetParticipantByUserIdRequest GetParticipantByUserIdRequest =
    new GetParticipantByUserIdRequest
    {
        UserId = UserId
    };
    protected static readonly UnregisterParticipantRequest UnregisterParticipantRequest =
    new UnregisterParticipantRequest
    {
        EventId = 1
    };
}
