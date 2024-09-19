using AutoMapper;
using Events.Application.DTOs.Events.Requests.CreateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEvent;
using Events.Application.DTOs.Events.Requests.UpdateEventsImage;
using Events.Application.DTOs.Events.Responces;
using Events.Application.Services;
using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using Moq;

namespace Events.Tests.Application.Services;

public class EventServiceTests : ApplicationTestBase
{
    private readonly EventService _service;
    protected readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IEventRepository> _eventRepositoryMock;

    public EventServiceTests() : base()
    {
        _mapperMock = new Mock<IMapper>();
        _eventRepositoryMock = new Mock<IEventRepository>();
        _service = new EventService(_eventRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllEventsAsync_ShouldReturnMappedEvents()
    {
        var events = new List<Event> { new Event { Id = 1, Name = "Event 1" } };
        var mappedEvents = new List<EventResponse> { new EventResponse { Id = 1, Name = "Event 1" } };

        _eventRepositoryMock.Setup(r => r.GetAllEventsAsync(1, 10)).ReturnsAsync(events);
        _mapperMock.Setup(m => m.Map<IEnumerable<EventResponse>>(events)).Returns(mappedEvents);

        var result = await _service.GetAllEventsAsync(1, 10);

        Assert.NotNull(result);
        Assert.Equal(mappedEvents.Count, result.Events.Count());
        _eventRepositoryMock.Verify(r => r.GetAllEventsAsync(1, 10), Times.Once);
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnMappedEvent_WhenEventExists()
    {
        var eventEntity = new Event { Id = 1, Name = "Event 1" };
        var mappedEvent = new EventResponse { Id = 1, Name = "Event 1" };

        _eventRepositoryMock.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(eventEntity);
        _mapperMock.Setup(m => m.Map<EventResponse>(eventEntity)).Returns(mappedEvent);

        var result = await _service.GetEventByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(mappedEvent.Name, result.Name);
        _eventRepositoryMock.Verify(r => r.GetEventByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        _eventRepositoryMock.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync((Event)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetEventByIdAsync(1));
    }

    [Fact]
    public async Task AddEventAsync_ShouldCallRepositoryWithMappedEvent()
    {
        var request = new CreateEventRequest { Name = "New Event" };
        var eventEntity = new Event { Name = "New Event" };

        _mapperMock.Setup(m => m.Map<Event>(request)).Returns(eventEntity);

        await _service.AddEventAsync(request);

        _eventRepositoryMock.Verify(r => r.AddEventAsync(eventEntity), Times.Once);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateEvent_WhenEventExists()
    {
        var request = new UpdateEventRequest { Name = "Updated Event" };
        var eventEntity = new Event { Id = 1, Name = "Event 1" };

        _eventRepositoryMock.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(eventEntity);

        await _service.UpdateEventAsync(1, request);

        _mapperMock.Verify(m => m.Map(request, eventEntity), Times.Once);
        _eventRepositoryMock.Verify(r => r.UpdateEventAsync(eventEntity), Times.Once);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var request = new UpdateEventRequest { Name = "Updated Event" };

        _eventRepositoryMock.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync((Event)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateEventAsync(1, request));
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldCallRepository()
    {
        var eventId = 1;

        await _service.DeleteEventAsync(eventId);

        _eventRepositoryMock.Verify(r => r.DeleteEventAsync(eventId), Times.Once);
    }

    [Fact]
    public async Task GetEventsByCriteriaAsync_ShouldReturnMappedEvents()
    {
        var events = new List<Event> { new Event { Id = 1, Name = "Event 1" } };
        var mappedEvents = new List<EventResponse> { new EventResponse { Id = 1, Name = "Event 1" } };

        _eventRepositoryMock.Setup(r => r.GetEventsByCriteriaAsync(null, null, null, 1, 10)).ReturnsAsync(events);
        _mapperMock.Setup(m => m.Map<IEnumerable<EventResponse>>(events)).Returns(mappedEvents);

        var result = await _service.GetEventsByCriteriaAsync(null, null, null, 1, 10);

        Assert.NotNull(result);
        Assert.Equal(mappedEvents.Count, result.Events.Count());
        _eventRepositoryMock.Verify(r => r.GetEventsByCriteriaAsync(null, null, null, 1, 10), Times.Once);
    }

    [Fact]
    public async Task UpdateEventsImageAsync_ShouldCallRepositoryWithCorrectParameters()
    {
        var request = new UpdateEventImageRequest { EventId = 1, ImageBytes = new byte[] { 0x01, 0x02 } };

        await _service.UpdateEventsImageAsync(request);

        _eventRepositoryMock.Verify(r => r.AddEventImageAsync(request.EventId, request.ImageBytes), Times.Once);
    }

    [Fact]
    public async Task GetEventsByUserIdAsync_ShouldReturnMappedEventsAndTotalCount()
    {
        var userId = Guid.NewGuid().ToString();
        var events = new List<Event>
        {
            new Event { Id = 1, Name = "Event 1", EventDateTime = DateTime.Now.AddDays(1) },
            new Event { Id = 2, Name = "Event 2", EventDateTime = DateTime.Now.AddDays(2) }
        };
        var mappedEvents = new List<EventResponse>
        {
            new EventResponse { Id = 1, Name = "Event 1" },
            new EventResponse { Id = 2, Name = "Event 2" }
        };
        var totalCount = 2;

        _eventRepositoryMock.Setup(r => r.GetEventsByUserIdAsync(userId, 1, 10)).ReturnsAsync(events);
        _eventRepositoryMock.Setup(r => r.GetUserEventsCountAsync(userId)).ReturnsAsync(totalCount);
        _mapperMock.Setup(m => m.Map<IEnumerable<EventResponse>>(events)).Returns(mappedEvents);

        var result = await _service.GetEventsByUserIdAsync(userId, 1, 10);

        Assert.NotNull(result);
        Assert.Equal(mappedEvents.Count, result.Events.Count());
        Assert.Equal(totalCount, result.TotalCount);
        _eventRepositoryMock.Verify(r => r.GetEventsByUserIdAsync(userId, 1, 10), Times.Once);
        _eventRepositoryMock.Verify(r => r.GetUserEventsCountAsync(userId), Times.Once);
    }

}
