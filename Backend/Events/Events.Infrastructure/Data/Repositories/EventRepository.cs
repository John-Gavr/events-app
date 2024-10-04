using Events.Core.Entities;
using Events.Core.Entities.Exceptions;
using Events.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public EventRepository(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> GetNumberOfAllEventsAsync()
    {
        return await _context.Events.CountAsync();
    }
    public async Task<int> GetNumberOfAllEventsByCriteriaAsync(DateTime? date = null, string? location = null, string? category = null)
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

        var result = query.Include(e => e.Participants);
        return result.Count();
    }
    public async Task<IEnumerable<Event>> GetAllEventsAsync(int pageNumber, int pageSize)
    {
        return await _context.Events.AsNoTracking()
                             .Include(e => e.Participants)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _context.Events.AsNoTracking().Include(e => e.Participants)
                                    .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Event?> GetEventByNameAsync(string name)
    {
        return await _context.Events.AsNoTracking().Include(e => e.Participants)
                                    .FirstOrDefaultAsync(e => e.Name == name);
    }

    public async Task AddEventAsync(Event newEvent)
    {
        await _context.Events.AddAsync(newEvent);
        await _unitOfWork.CompleteAsync();
    }

    public async Task UpdateEventAsync(Event updatedEvent)
    {
        _context.Events.Update(updatedEvent);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteEventAsync(int id)
    {
        var eventToDelete = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        
        _context.Events.Remove(eventToDelete!);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsByCriteriaAsync(DateTime? date = null, string? location = null, string? category = null, int pageNumber = 1, int pageSize = 10)
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
                          .ToListAsync();
    }

    public async Task AddEventImageAsync(int id, byte[] image)
    {
        var eventToUpdate = await _context.Events.FindAsync(id);

        eventToUpdate!.Image = image;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(string userId, int pageNumber, int pageSize)
    {
        return await _context.Events.AsNoTracking()
            .Where(e => e.Participants.Any(p => p.UserId.ToString().Equals(userId)))
            .OrderBy(e => e.EventDateTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetUserEventsCountAsync(string userId)
    {
        return await _context.Events.AsNoTracking()
            .Where(e => e.Participants.Any(p => p.UserId.ToString().Equals(userId))).CountAsync();
    }
}
