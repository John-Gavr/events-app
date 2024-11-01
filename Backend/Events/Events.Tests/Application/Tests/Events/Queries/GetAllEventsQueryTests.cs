using Events.Application.UseCases.Events.DTOs;
using Events.Application.UseCases.Events.Queries.GetAllEvents;
using Events.Core.Entities;
using Moq;

namespace Events.Tests.Application.Tests.Events.Queries;

public class GetAllEventsQueryTests : EventsTestBase
{
    private readonly GetAllEventsQueryHandler _handler;

    public GetAllEventsQueryTests()
    {
        _handler = new(_eventRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllEventsAsync_ShouldReturnEventsResponse()
    {
        _eventRepositoryMock.Setup(x => x.GetAllEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Event> { testEvent });
        _eventRepositoryMock.Setup(x => x.GetNumberOfAllEventsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _mapperMock.Setup(x => x.Map<IEnumerable<EventResponseDTO>>(It.IsAny<IEnumerable<Event>>()))
        .Returns(eventsResponseDTO.Events);

        var result = await _handler.Handle(getAllEventsQuery, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(eventsResponseDTO.TotalCount, result.TotalCount);
        Assert.Equal(eventsResponseDTO.Events, result.Events);
    }
}
