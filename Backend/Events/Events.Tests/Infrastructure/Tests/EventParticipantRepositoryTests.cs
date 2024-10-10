using Events.Infrastructure.Data.Repositories;
using Events.Tests.Infrastructure.Extensions;
using Events.Tests.Infrastructure.TestBases;
using Microsoft.EntityFrameworkCore;

namespace Events.Tests.Infrastructure.Tests;
public class EventParticipantRepositoryTests : EventParticipantRepositoryTestBase
{
    private readonly EventParticipantRepository _repository;
    private readonly ParticipantRepositoryExtension _participantRepositoryExtension;
    private readonly CancellationToken _cancellationToken;

    public EventParticipantRepositoryTests()
    {
        _repository = new EventParticipantRepository(_context);
        _participantRepositoryExtension = new ParticipantRepositoryExtension(_context);
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task RegisterParticipantAsync_Success()
    {
        await _repository.RegisterParticipantAsync(EventIdToRegisterParticipant, ParticipantEntityToRegister, _cancellationToken);

        var eventWithParticipants = await _context.Events
                                                  .Include(e => e.Participants)
                                                  .FirstOrDefaultAsync(e => e.Id == EventIdToRegisterParticipant, _cancellationToken);

        Assert.NotNull(eventWithParticipants);
        Assert.Contains(eventWithParticipants.Participants, p =>
            p.FirstName == ParticipantEntityToRegister.FirstName &&
            p.LastName == ParticipantEntityToRegister.LastName &&
            p.Email == ParticipantEntityToRegister.Email &&
            p.UserId == ParticipantEntityToRegister.UserId &&
            p.EventId == EventIdToRegisterParticipant);
    }

    [Fact]
    public async Task GetParticipantsByEventIdAsync_ShouldReturnParticipants()
    {
        _participantRepositoryExtension.GetParticipantsToEventAsync(EventIdToGetParticipants);

        var result = await _repository.GetParticipantsByEventIdAsync(EventIdToGetParticipants, 1, 10, _cancellationToken);

        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task GetParticipantsByEventIdAsync_ShouldReturnPaginatedEventParticipant()
    {
        _participantRepositoryExtension.GetParticipantsToEventAsync(EventIdToGetParticipants);

        var result = await _repository.GetParticipantsByEventIdAsync(EventIdToGetParticipants, 1, 3, _cancellationToken);

        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetParticipantByUserIdAsync_ShouldReturnParticipant()
    {           
        var result = await _repository.GetParticipantByUserIdAsync(UserIdToGetParticipant, _cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(UserIdToGetParticipant, result!.UserId.ToString());
    }
    [Fact]
    public async Task UnregisterParticipantAsync_Success()
    {
        await _repository.UnregisterParticipantAsync(EventIdToUnregisterUser, UserIdToUnregister, _cancellationToken);

        var eventEntity = await _context.Events.Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == EventIdToUnregisterUser, _cancellationToken);
        var participantEntity = eventEntity!.Participants.FirstOrDefault(p => p.UserId.ToString().Equals(UserIdToUnregister));

        Assert.Null(participantEntity);
    }
}
