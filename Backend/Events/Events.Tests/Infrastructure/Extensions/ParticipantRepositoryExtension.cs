using Events.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Events.Tests.Infrastructure.Extensions;

public class ParticipantRepositoryExtension
{
    private readonly AppDbContext _context;

    public ParticipantRepositoryExtension(AppDbContext context)
    {
        _context = context;
    }   

    public async void GetParticipantsToEventAsync(int eventId)
    {
        var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);

        eventEntity!.Participants = _context.EventParticipants.Where(p => p.EventId == eventId).ToList();
    }
}
