using Events.Core.Entities.Exceptions;
using Events.Core.Entities;
using Events.Application.UseCases.Events.Queries.GetEventById;
using Moq;
using Events.Application.UseCases.Events.DTOs;

namespace Events.Tests.Application.Tests.Events.Queries;

public class GetEventByIdQueryTests : EventsTestBase
{
    private readonly GetEventByIdQueryHandler _handler;

    public GetEventByIdQueryTests()
    {
        _handler = new(_eventRepositoryMock.Object, _mapperMock.Object);
    }
    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnEventResponse()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testEvent);
        _mapperMock.Setup(x => x.Map<EventResponseDTO>(It.IsAny<Event>()))
            .Returns(eventsResponseDTO.Events.First());

        var result = await _handler.Handle(getEventByIdQuery, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(eventsResponseDTO.Events.First().Id, result.Id);
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(getEventByIdQuery, _cancellationToken));
    }
}
