using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using Infrastructure.Data;
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

        if (eventEntity != null)
        {
            return eventEntity.Participants
                              .Skip((pageNumber - 1) * pageSize) 
                              .Take(pageSize)
                              .ToList();
        }

        return new List<EventParticipant>();
    }

    public async Task<EventParticipant?> GetParticipantByIdAsync(int participantId)
    {
        var participantEntity = await _context.EventParticipants.FindAsync(participantId);
        if(participantEntity == null)
            throw new NotFoundException(nameof(participantEntity), participantId);
        
        return participantEntity;
    }

    public async Task UnregisterParticipantAsync(int eventId, int participantId)
    {
        var eventEntity = await _context.Events.Include(e => e.Participants)
                                               .FirstOrDefaultAsync(e => e.Id == eventId);
        if (eventEntity == null)
            throw new NotFoundException(nameof(eventEntity), eventId);

        var participantEntity = eventEntity?.Participants.FirstOrDefault(p => p.Id == participantId);
        if (participantEntity == null)
            throw new NotFoundException(nameof(participantEntity), participantId);

        eventEntity!.Participants.Remove(participantEntity);
        await _unitOfWork.CompleteAsync();
    }
}
