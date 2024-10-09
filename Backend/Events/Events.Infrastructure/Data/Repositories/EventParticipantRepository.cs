using Events.Core.Entities;
using Events.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class EventParticipantRepository : IEventParticipantRepository
{
    private readonly AppDbContext _context;

    public EventParticipantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegisterParticipantAsync(int eventId, EventParticipant participant, CancellationToken cancellationToken)
    {
        var eventEntity = await _context.Events.Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        eventEntity!.Participants.Add(participant);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<EventParticipant>> GetParticipantsByEventIdAsync(int eventId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var eventEntity = await _context.Events.AsNoTracking().Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        return eventEntity!.Participants
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public async Task<EventParticipant?> GetParticipantByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.EventParticipants.FirstOrDefaultAsync(p => p.UserId.ToString().Equals(userId), cancellationToken);
    }

    public async Task UnregisterParticipantAsync(int eventId, string userId, CancellationToken cancellationToken)
    {
        var eventEntity = await _context.Events.Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        var participantEntity = eventEntity?.Participants.FirstOrDefault(p => p.UserId.ToString().Equals(userId));
        eventEntity!.Participants.Remove(participantEntity!);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
