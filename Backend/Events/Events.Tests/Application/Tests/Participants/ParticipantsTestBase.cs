using Events.Application.UseCases.Participants.Commands.RegisterParticipant;
using Events.Application.UseCases.Participants.Commands.UnregisterParticipant;
using Events.Application.UseCases.Participants.Queries.GetParticipantByUserId;
using Events.Application.UseCases.Participants.Queries.GetParticipantsByEventId;
using Events.Core.Entities;
using Events.Core.Interfaces;
using Moq;

namespace Events.Tests.Application.Tests.Participants;

public class ParticipantsTestBase : ApplicationTestBase
{
    protected readonly Mock<IEventParticipantRepository> _eventParticipantRepositoryMock;
    protected readonly Mock<IEventRepository> _eventRepositoryMock;
    public ParticipantsTestBase()
    {
        _eventParticipantRepositoryMock = new Mock<IEventParticipantRepository>();
        _eventRepositoryMock = new Mock<IEventRepository>();
    }
    protected static readonly RegisterParticipantCommand registerParticipantCommand = new RegisterParticipantCommand { EventId = 1, UserId = userId };
    protected static readonly UnregisterParticipantCommand unregisterParticipantCommand = new UnregisterParticipantCommand { EventId = 1, UserId = userId };
    protected static readonly EventParticipant testParticipant = new EventParticipant
    {
        EventId = registerParticipantCommand.EventId,
        UserId = Guid.Parse(userId)
    };
    protected static readonly GetParticipantsByEventIdQuery getParticipantsByEventIdQuery =
        new GetParticipantsByEventIdQuery
        {
            EventId = 1,
            PageNumber = 1,
            PageSize = 10
        };
    protected static readonly GetParticipantByUserIdQuery getParticipantByUserIdQuery = new GetParticipantByUserIdQuery { UserId = userId };
}
