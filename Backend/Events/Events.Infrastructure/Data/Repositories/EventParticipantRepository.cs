using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
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
        var eventEntity = await _context.Events.Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == eventId);

        if (eventEntity == null)
        {
            throw new NotFoundException(nameof(eventEntity), eventId);
        }

        if (eventEntity.Participants.Count >= eventEntity.MaxParticipants)
        {
            throw new InvalidOperationException("The event has reached it's maximum number of participants.");
        }

        eventEntity.Participants.Add(participant);
        await _unitOfWork.CompleteAsync();
    }


    public async Task<IEnumerable<EventParticipant>> GetParticipantsByEventIdAsync(int eventId, int pageNumber, int pageSize)
    {
        var eventEntity = await _context.Events.Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == eventId);

        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), eventId);

         return eventEntity.Participants
            .Skip((pageNumber - 1) * pageSize) 
            .Take(pageSize)
            .ToList();
    }

    public async Task<EventParticipant?> GetParticipantByUserIdAsync(string userId)
    {
        var participantEntity = await _context.EventParticipants.FirstOrDefaultAsync(p => p.UserId.ToString().Equals(userId));
        if(participantEntity == null)
            throw new NotFoundException(nameof(participantEntity), userId);
        
        return participantEntity;
    }

    public async Task UnregisterParticipantAsync(int eventId, string userId)
    {
        var eventEntity = await _context.Events.Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == eventId);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), eventId);

        var participantEntity = eventEntity?.Participants.FirstOrDefault(p => p.UserId.ToString().Equals(userId));
        if (participantEntity == null)
            throw new NotFoundException(nameof(participantEntity), userId);

        eventEntity!.Participants.Remove(participantEntity);
        await _unitOfWork.CompleteAsync();
    }
}
