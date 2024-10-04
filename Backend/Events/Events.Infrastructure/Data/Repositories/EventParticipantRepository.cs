using Events.Core.Entities;
using Events.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class EventParticipantRepository : IEventParticipantRepository
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public EventParticipantRepository(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task RegisterParticipantAsync(int eventId, EventParticipant participant)
    {
        var eventEntity = await _context.Events.AsNoTracking().Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == eventId);

        eventEntity!.Participants.Add(participant);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<EventParticipant>> GetParticipantsByEventIdAsync(int eventId, int pageNumber, int pageSize)
    {
        var eventEntity = await _context.Events.AsNoTracking().Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == eventId);

         return eventEntity!.Participants
            .Skip((pageNumber - 1) * pageSize) 
            .Take(pageSize)
            .ToList();
    }

    public async Task<EventParticipant?> GetParticipantByUserIdAsync(string userId)
    {
        return await _context.EventParticipants.FirstOrDefaultAsync(p => p.UserId.ToString().Equals(userId));
    }

    public async Task UnregisterParticipantAsync(int eventId, string userId)
    {
        var eventEntity = await _context.Events.Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == eventId);

        var participantEntity = eventEntity?.Participants.FirstOrDefault(p => p.UserId.ToString().Equals(userId));

        eventEntity!.Participants.Remove(participantEntity!);
        await _context.SaveChangesAsync();
    }
}
