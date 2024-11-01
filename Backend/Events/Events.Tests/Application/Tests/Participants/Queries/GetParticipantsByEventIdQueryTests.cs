using Events.Application.UseCases.Participants.DTOs;
using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Moq;
using Events.Application.UseCases.Participants.Queries.GetParticipantsByEventId;

namespace Events.Tests.Application.Tests.Participants.Queries;

public class GetParticipantsByEventIdQueryTests : ParticipantsTestBase
{
    private readonly GetParticipantsByEventIdQueryHandler _handler;

    public GetParticipantsByEventIdQueryTests()
    {
        _handler = new(_mapperMock.Object, _eventParticipantRepositoryMock.Object, _userManagerMock.Object, _eventRepositoryMock.Object);
    }

    [Fact]
    public async Task GetParticipantsByEventIdAsync_ShouldReturnParticipants_WhenEventExists()
    {
        _eventRepositoryMock.Setup(e => e.GetEventByIdAsync(getParticipantsByEventIdQuery.EventId, _cancellationToken)).ReturnsAsync(testEvent);
        _eventParticipantRepositoryMock.Setup(p => p.GetParticipantsByEventIdAsync(getParticipantsByEventIdQuery.EventId,
            getParticipantsByEventIdQuery.PageNumber,
            getParticipantsByEventIdQuery.PageSize,
            _cancellationToken)).ReturnsAsync(ParticipantsList);
        _mapperMock.Setup(m => m.Map<IEnumerable<EventParticipantResponseDTO>>(ParticipantsList)).Returns(new List<EventParticipantResponseDTO>());

        var result = await _handler.Handle(getParticipantsByEventIdQuery, _cancellationToken);

        Assert.NotNull(result);
        _mapperMock.Verify(m => m.Map<IEnumerable<EventParticipantResponseDTO>>(ParticipantsList), Times.Once);
    }

    [Fact]
    public async Task GetParticipantsByEventIdAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        _eventRepositoryMock.Setup(e =>
        e.GetEventByIdAsync(getParticipantsByEventIdQuery.EventId, _cancellationToken)).ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(getParticipantsByEventIdQuery,
            _cancellationToken));
    }
}
