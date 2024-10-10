using Events.Infrastructure.Data.Repositories;
using Events.Tests.Infrastructure.TestBases;
using Microsoft.EntityFrameworkCore;

namespace Events.Tests.Infrastructure.Tests;
public class EventRepositoryTests : EventRepositoryTestBase
{
    private readonly EventRepository _repository;
    private readonly CancellationToken _cancellationToken;
    public EventRepositoryTests()
    {
        _repository = new EventRepository(_context);
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task GetAllEventsAsync_ShouldReturnPaginatedEvents()
    {
        var result = await _repository.GetAllEventsAsync(PageNumber, PageSize, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnEvent_WhenEventExists()
    {
        var result = await _repository.GetEventByIdAsync(EventIdToGetWhenExist, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(EventIdToGetWhenExist, result!.Id);
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnNull_WhenEventDoesNotExist()
    {
        var result = await _repository.GetEventByIdAsync(EventIdToGetWhenNotExist, _cancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddEventAsync_ShouldAddNewEvent()
    {
        await _repository.AddEventAsync(NewEventToAdd, _cancellationToken);
        var result = await _context.Events.FirstOrDefaultAsync(e => e.Name == "New Event");

        Assert.NotNull(result);
        Assert.Equal(NewEventToAdd.Name, result!.Name);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateExistingEvent()
    {
        var eventToUpdate = await _context.Events.FindAsync(2);
        Assert.NotNull(eventToUpdate);

        eventToUpdate!.Name = UpdatedValuesForEvent.Name;
        eventToUpdate.EventDateTime = UpdatedValuesForEvent.EventDateTime;
        eventToUpdate.Location = UpdatedValuesForEvent.Location;
        eventToUpdate.MaxParticipants = UpdatedValuesForEvent.MaxParticipants;
        eventToUpdate.Category = UpdatedValuesForEvent.Category;
        eventToUpdate.Description = UpdatedValuesForEvent.Description;

        await _repository.UpdateEventAsync(eventToUpdate, _cancellationToken);
        var updatedEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventToUpdate.Id);

        Assert.NotNull(updatedEvent);
        Assert.Equal(UpdatedValuesForEvent.Name, updatedEvent!.Name);
        Assert.Equal(UpdatedValuesForEvent.EventDateTime, updatedEvent.EventDateTime);
        Assert.Equal(UpdatedValuesForEvent.Location, updatedEvent.Location);
        Assert.Equal(UpdatedValuesForEvent.MaxParticipants, updatedEvent.MaxParticipants);
        Assert.Equal(UpdatedValuesForEvent.Category, updatedEvent.Category);
        Assert.Equal(UpdatedValuesForEvent.Description, updatedEvent.Description);
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldRemoveEvent()
    {
        await _repository.DeleteEventAsync(EventIdForDelete, _cancellationToken);
        var result = await _context.Events.FindAsync(EventIdForDelete);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetEventsByCriteriaAsync_ShouldReturnEventsByDate()
    {
        var result = await _repository.GetEventsByCriteriaAsync(_cancellationToken, date: DateCriteria);

        Assert.NotNull(result);
        Assert.All(result, e => Assert.Equal(DateCriteria, e.EventDateTime.Date));
    }

    [Fact]
    public async Task GetEventsByCriteriaAsync_ShouldReturnEventsByLocation()
    {
        var result = await _repository.GetEventsByCriteriaAsync(_cancellationToken, location: LocationCriteria);

        Assert.NotNull(result);
        Assert.All(result, e => Assert.Equal(LocationCriteria, e.Location));
    }

    [Fact]
    public async Task AddEventImageAsync_ShouldUpdateEventImage()
    {
        await _repository.AddEventImageAsync(EventIdForUpdateImage, ImageBytes, _cancellationToken);
        var updatedEvent = await _context.Events.FindAsync(EventIdForUpdateImage);

        Assert.NotNull(updatedEvent);
        Assert.Equal(ImageBytes, updatedEvent!.Image);
    }

    [Fact]
    public async Task GetEventsByUserIdAsync_ReturnsEventsForGivenUserId()
    {
        var result = await _repository.GetEventsByUserIdAsync(UserIdToGetRelatedEventsOrCount, PageNumber, PageSize, _cancellationToken);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetUserEventsCountAsync_ReturnsCorrectCount()
    {
        var count = await _repository.GetUserEventsCountAsync(UserIdToGetRelatedEventsOrCount, _cancellationToken);

        Assert.Equal(1, count);
    }
}
