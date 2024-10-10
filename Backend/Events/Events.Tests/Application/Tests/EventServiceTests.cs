using Events.Application.DTOs.Events.Requests.CreateEvent;
using Events.Application.DTOs.Events.Responces;
using Events.Application.Services;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using Events.Tests.Application.TestBases;
using Moq;

namespace Events.Tests.Application.Services.Tests;
public class EventServiceTests : EventServiceTestBase
{
    private readonly EventService _eventService;

    protected readonly Mock<IEventRepository> _eventRepositoryMock;
    public EventServiceTests()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();
        _eventService = new EventService(
            _eventRepositoryMock.Object,
            _mapperMock.Object,
            _userManagerMock.Object
        );
    }

    [Fact]
    public async Task GetAllEventsAsync_ShouldReturnEventsResponse()
    {
        _eventRepositoryMock.Setup(x => x.GetAllEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Event> { TestEvent });
        _eventRepositoryMock.Setup(x => x.GetNumberOfAllEventsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _mapperMock.Setup(x => x.Map<IEnumerable<EventResponse>>(It.IsAny<IEnumerable<Event>>()))
            .Returns(EventsResponse.Events);

        var result = await _eventService.GetAllEventsAsync(1, 10, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(EventsResponse.TotalCount, result.TotalCount);
        Assert.Equal(EventsResponse.Events, result.Events);
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnEventResponse()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestEvent);
        _mapperMock.Setup(x => x.Map<EventResponse>(It.IsAny<Event>()))
            .Returns(EventsResponse.Events.First());

        var result = await _eventService.GetEventByIdAsync(1, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(EventsResponse.Events.First().Id, result.Id);
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _eventService.GetEventByIdAsync(1, _cancellationToken));
    }

    [Fact]
    public async Task AddEventAsync_ShouldAddEvent()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null);

        _mapperMock.Setup(x => x.Map<Event>(It.IsAny<CreateEventRequest>()))
            .Returns(TestEvent);

        await _eventService.AddEventAsync(CreateEventRequest, _cancellationToken);

        _eventRepositoryMock.Verify(x => x.AddEventAsync(It.IsAny<Event>(), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task AddEventAsync_ShouldThrowEventAlredyExistException_WhenEventAlreadyExists()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestEvent);

        await Assert.ThrowsAsync<EventAlredyExistException>(() => _eventService.AddEventAsync(CreateEventRequest, _cancellationToken));
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateEvent()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestEvent);

        await _eventService.UpdateEventAsync(1, UpdateEventRequest, _cancellationToken);

        _eventRepositoryMock.Verify(x => x.UpdateEventAsync(It.IsAny<Event>(), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _eventService.UpdateEventAsync(1, UpdateEventRequest, _cancellationToken));
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldDeleteEvent()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestEvent);

        await _eventService.DeleteEventAsync(1, _cancellationToken);

        _eventRepositoryMock.Verify(x => x.DeleteEventAsync(1, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _eventService.DeleteEventAsync(1, _cancellationToken));
    }

    [Fact]
    public async Task UpdateEventsImageAsync_ShouldUpdateImage()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestEvent);

        await _eventService.UpdateEventsImageAsync(UpdateEventImageRequest, _cancellationToken);

        _eventRepositoryMock.Verify(x => x.AddEventImageAsync(It.IsAny<int>(), It.IsAny<byte[]>(), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateEventsImageAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        _eventRepositoryMock.Setup(x => x.GetEventByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _eventService.UpdateEventsImageAsync(UpdateEventImageRequest, _cancellationToken));
    }
}
