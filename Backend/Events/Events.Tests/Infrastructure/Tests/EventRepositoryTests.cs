using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using Events.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Events.Tests.Infrastructure.Tests;

public class EventRepositoryTests : InfrastructureTestBase
{
    private readonly EventRepository _repository;

    public EventRepositoryTests()
    {
        _repository = new EventRepository(_context, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetAllEventsAsync_ShouldReturnPaginatedEvents()
    {
        var result = await _repository.GetAllEventsAsync(1, 10);

        Assert.NotNull(result);
        Assert.True(result.Any());
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnEvent_WhenEventExists()
    {
        var eventId = 1; 

        var result = await _repository.GetEventByIdAsync(eventId);

        Assert.NotNull(result);
        Assert.Equal(result, _context.Events.Find(eventId));
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnNull_WhenEventDoesNotExist()
    {
        var eventId = 999;

        var result = await _repository.GetEventByIdAsync(eventId);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddEventAsync_ShouldAddNewEvent()
    {
        var newEvent = new Event
        {
            Id = 3,
            Name = "New Event",
            Description = "Test description",
            EventDateTime = DateTime.Parse("2024-10-10"),
            Location = "Test Location",
            MaxParticipants = 100
        };

        await _repository.AddEventAsync(newEvent);
        var result = await _context.Events.FirstOrDefaultAsync(e => e.Name == "New Event");

        Assert.NotNull(result);
        Assert.Equal("New Event", result.Name);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateExistingEvent()
    {
        var updatedValues = new
        {
            Name = "Updated Event",
            Date = DateTime.Parse("2025-01-25"),
            Location = "Updated Location",
            MaxParticipants = 150,
            Category = "Updated Category",
            Description = "Updated Description"
        };

        var eventToUpdate = await _context.Events.FindAsync(2);
        Assert.NotNull(eventToUpdate); 

        eventToUpdate!.Name = updatedValues.Name;
        eventToUpdate.EventDateTime = updatedValues.Date;
        eventToUpdate.Location = updatedValues.Location;
        eventToUpdate.MaxParticipants = updatedValues.MaxParticipants;
        eventToUpdate.Category = updatedValues.Category;
        eventToUpdate.Description = updatedValues.Description;

        await _repository.UpdateEventAsync(eventToUpdate);
        var updatedEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventToUpdate.Id);

        Assert.NotNull(updatedEvent);
        Assert.Equal(updatedValues.Name, updatedEvent!.Name);
        Assert.Equal(updatedValues.Date, updatedEvent.EventDateTime);
        Assert.Equal(updatedValues.Location, updatedEvent.Location);
        Assert.Equal(updatedValues.MaxParticipants, updatedEvent.MaxParticipants);
        Assert.Equal(updatedValues.Category, updatedEvent.Category);
        Assert.Equal(updatedValues.Description, updatedEvent.Description);
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldRemoveEvent_WhenEventExists()
    {
        var eventId = 2; 

        await _repository.DeleteEventAsync(eventId);
        var result = await _context.Events.FindAsync(eventId);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = 999;

        await Assert.ThrowsAsync<NotFoundException>(() => _repository.DeleteEventAsync(eventId));
    }

    [Fact]
    public async Task GetEventsByCriteriaAsync_ShouldReturnEventsByDate()
    {
        var date = DateTime.Parse("2024-10-10"); 

        var result = await _repository.GetEventsByCriteriaAsync(date: date);

        Assert.NotNull(result);
        Assert.All(result, e => Assert.Equal(date, e.EventDateTime.Date));
    }
    [Fact]
    public async Task GetEventsByCriteriaAsync_ShouldReturnEventsByLocation()
    {
        var location = "Location of the second event"; 

        var result = await _repository.GetEventsByCriteriaAsync(location: location);

        Assert.NotNull(result);
        Assert.All(result, e => Assert.Equal(location, e.Location));
    }

    [Fact]
    public async Task AddEventImageAsync_ShouldUpdateEventImage()
    {
        var eventId = 1;
        var imageBytes = new byte[] { 1, 2, 3, 4 };

        await _repository.AddEventImageAsync(eventId, imageBytes);
        var updatedEvent = await _context.Events.FindAsync(eventId);

        Assert.NotNull(updatedEvent);
        Assert.Equal(imageBytes, updatedEvent.Image);
    }
    
    [Fact]
    public async Task AddEventImageAsync_ShouldhrowNotFoundException_WhenEventDoesNotExist()
    {
        var eventId = 99;
        var imageBytes = new byte[] { 1, 2, 3, 4 };

        await Assert.ThrowsAsync<NotFoundException>(() => _repository.AddEventImageAsync(eventId, imageBytes));
    }

    [Fact]
    public async Task GetEventsByUserIdAsync_ReturnsEventsForGivenUserId()
    {
        var userId = Guid.NewGuid().ToString();
        var participant1 = new EventParticipant { Id = 27, UserId = Guid.Parse(userId) };
        var participant2 = new EventParticipant { Id = 28, UserId = Guid.NewGuid() };

        var event1 = new Event { Id = 23, EventDateTime = DateTime.Now.AddDays(1), Participants = new List<EventParticipant> { participant1 } };
        var event2 = new Event { Id = 24, EventDateTime = DateTime.Now.AddDays(2), Participants = new List<EventParticipant> { participant2 } };

        _context.Events.AddRange(event1, event2);
        await _context.SaveChangesAsync();

        int pageNumber = 1;
        int pageSize = 2;

        var result = await _repository.GetEventsByUserIdAsync(userId, pageNumber, pageSize);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, e => e.EventDateTime == event1.EventDateTime);
    }

    [Fact]
    public async Task GetUserEventsCountAsync_ReturnsCorrectCount()
    {
        var userId = Guid.NewGuid().ToString();
        var participant1 = new EventParticipant { Id = 37, UserId = Guid.Parse(userId) };
        var participant2 = new EventParticipant { Id = 38, UserId = Guid.NewGuid() };

        var event1 = new Event { Id = 33, EventDateTime = DateTime.Now.AddDays(1), Participants = new List<EventParticipant> { participant1 } };
        var event2 = new Event { Id = 34, EventDateTime = DateTime.Now.AddDays(2), Participants = new List<EventParticipant> { participant2 } };

        _context.Events.AddRange(event1, event2);
        await _context.SaveChangesAsync();

        var count = await _repository.GetUserEventsCountAsync(userId);

        Assert.Equal(1, count); 
    }

}
