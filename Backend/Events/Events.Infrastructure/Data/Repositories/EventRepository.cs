using Events.Core.Entities;
using Events.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetNumberOfAllEventsAsync(CancellationToken cancellationToken)
    {
        return await _context.Events.CountAsync(cancellationToken);
    }

    public async Task<int> GetNumberOfAllEventsByCriteriaAsync(CancellationToken cancellationToken, DateTime? date = null, string? location = null, string? category = null)
    {
        var query = _context.Events.AsNoTracking().AsQueryable();

        if (date.HasValue)
        {
            query = query.Where(e => e.EventDateTime.Date == date.Value.Date);
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(e => e.Location == location);
        }

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(e => e.Category == category);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Events.AsNoTracking()
                             .Include(e => e.Participants)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync(cancellationToken);
    }

    public async Task<Event?> GetEventByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Events.AsNoTracking()
                                    .Include(e => e.Participants)
                                    .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Event?> GetEventByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Events.AsNoTracking()
                                    .Include(e => e.Participants)
                                    .FirstOrDefaultAsync(e => e.Name == name, cancellationToken);
    }

    public async Task AddEventAsync(Event newEvent, CancellationToken cancellationToken)
    {
        await _context.Events.AddAsync(newEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateEventAsync(Event updatedEvent, CancellationToken cancellationToken)
    {
        _context.Events.Update(updatedEvent);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteEventAsync(int id, CancellationToken cancellationToken)
    {
        var eventToDelete = await _context.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        _context.Events.Remove(eventToDelete!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetEventsByCriteriaAsync(CancellationToken cancellationToken, DateTime? date = null, string? location = null, string? category = null, int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.Events.AsNoTracking().AsQueryable();

        if (date.HasValue)
        {
            query = query.Where(e => e.EventDateTime.Date == date.Value.Date);
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(e => e.Location == location);
        }

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(e => e.Category == category);
        }

        return await query.Include(e => e.Participants)
                          .Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync(cancellationToken);
    }

    public async Task AddEventImageAsync(int id, byte[] image, CancellationToken cancellationToken)
    {
        var eventToUpdate = await _context.Events.FindAsync(new object[] { id }, cancellationToken);

        eventToUpdate!.Image = image;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Events.AsNoTracking()
            .Where(e => e.Participants.Any(p => p.UserId.ToString().Equals(userId)))
            .OrderBy(e => e.EventDateTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUserEventsCountAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.Events.AsNoTracking()
            .Where(e => e.Participants.Any(p => p.UserId.ToString().Equals(userId)))
            .CountAsync(cancellationToken);
    }
}
