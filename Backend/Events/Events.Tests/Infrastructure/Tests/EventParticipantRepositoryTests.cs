using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Infrastructure.Data.Repositories;
using Events.Tests.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Events.Tests.Infrastructure.Tests;

public class EventParticipantRepositoryTests : InfrastructureTestBase
{
    private readonly EventParticipantRepository _repository;
    private readonly ParticipantRepositoryExtension _participantRepositoryExtension;

    public EventParticipantRepositoryTests() 
    {
        _repository = new EventParticipantRepository(_context, _unitOfWorkMock.Object);
        _participantRepositoryExtension = new ParticipantRepositoryExtension(_context);
    }

    [Fact]
    public async Task RegisterParticipantAsync_Success()
    {
        int eventId = 2;
        var participantEntity = new EventParticipant()
        {
            FirstName = "John",
            LastName = "Brown",
            DateOfBirth = DateTime.Parse("22-04-2001"),
            RegistrationDate = DateTime.Now,
            Email = "john@gmail.com",
            UserId = Guid.NewGuid(),
            EventId = eventId
        };

        await _repository.RegisterParticipantAsync(eventId, participantEntity);

        var eventWithParticipants = await _context.Events
                                              .Include(e => e.Participants)
                                              .FirstOrDefaultAsync(e => e.Id == eventId);

        Assert.NotNull(eventWithParticipants);

        Assert.Contains(eventWithParticipants.Participants, p =>
            p.FirstName == participantEntity.FirstName &&
            p.LastName == participantEntity.LastName &&
            p.Email == participantEntity.Email &&
            p.UserId == participantEntity.UserId &&
            p.EventId == eventId);
    }

    [Fact]
    public async Task RegisterParticipantAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        int eventId = 999;
        var participantEntity = new EventParticipant();

        await Assert.ThrowsAsync<NotFoundException>(() => _repository.RegisterParticipantAsync(eventId, participantEntity));
    }

    [Fact]
    public async Task RegisterParticipantAsync_ShouldThrowInvalidOperationException_WhenMaxParticipantsReached()
    {
        _participantRepositoryExtension.GetParticipantsToEventAsync(1);

        int eventId = 1;
        var participantEntity = new EventParticipant()
        {
            FirstName = "John",
            LastName = "Brown",
            DateOfBirth = DateTime.Parse("22-04-2001"),
            RegistrationDate = DateTime.Now,
            Email = "john@gmail.com",
            UserId = Guid.NewGuid(),
            EventId = eventId
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.RegisterParticipantAsync(eventId, participantEntity));
    }

    [Fact]
    public async Task GetParticipantsByEventIdAsync_ShouldReturnParticipants_WhenEventExists()
    {
        _participantRepositoryExtension.GetParticipantsToEventAsync(1);

        var result = await _repository.GetParticipantsByEventIdAsync(1, 1, 10);

        Assert.Equal(5, result.Count());
    }
    
    [Fact]
    public async Task GetParticipantsByEventIdAsync_ShouldReturnPaginatedEventParticipant_WhenEventExists()
    {
        _participantRepositoryExtension.GetParticipantsToEventAsync(1);

        var result = await _repository.GetParticipantsByEventIdAsync(1, 1, 3);

        Assert.Equal(3, result.Count());
    }
    
    [Fact]
    public async Task GetParticipantsByEventIdAsync_ShouldThrowInvalidOperationException_WhenEventNotFound()
    {
        int eventId = 99;

        await Assert.ThrowsAsync<NotFoundException>(() => _repository.GetParticipantsByEventIdAsync(eventId, 1, 10));
    }

    [Fact]
    public async Task UnregisterParticipantAsync_ShouldThrowNotFoundException_WhenParticipantNotFound()
    {
        int eventId = 1;

        await Assert.ThrowsAsync<NotFoundException>(() => _repository.UnregisterParticipantAsync(eventId, Guid.NewGuid().ToString()));
    }
    [Fact]
    public async Task UnregisterParticipantAsync_ShouldThrowNotFoundException_WhenEventNotFound()
    {
        int eventId = 99;

        await Assert.ThrowsAsync<NotFoundException>(() => _repository.UnregisterParticipantAsync(eventId, Guid.NewGuid().ToString()));
    }
    [Fact]
    public async Task UnregisterParticipantAsync_Success()
    {
        int eventId = 2;
        string userId = "E435148A-5CD8-4513-BA51-2C8B4D091684";

        await Assert.ThrowsAsync<NotFoundException>(() => _repository.UnregisterParticipantAsync(eventId, userId));
    }
}
