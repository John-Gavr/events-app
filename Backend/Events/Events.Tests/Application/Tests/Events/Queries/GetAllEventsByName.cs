using Events.Application.UseCases.Events.DTOs;
using Events.Application.UseCases.Events.Queries.GetEventByName;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Moq;

namespace Events.Tests.Application.Tests.Events.Queries;

public class GetAllEventsByName : EventsTestBase
{

    private readonly GetEventByNameQueryHandler _handler;

    public GetAllEventsByName()
    {
        _handler = new(_mapperMock.Object, _eventRepositoryMock.Object);
    }

    [Fact]
    public async Task GetEventByNameAsync_ShouldReturnEvent()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testEvent);
        _mapperMock.Setup(x => x.Map<EventResponseDTO>(It.IsAny<Event>()))
        .Returns(eventResponseDTO);

        var result = await _handler.Handle(getEventByNameQuery, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(getEventByNameQuery.Name, result.Name);
        Assert.Equal(1, result.Id);
    }
    
    [Fact]
    public async Task GetEventByNameAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(getEventByNameQuery, _cancellationToken));
    }
}
