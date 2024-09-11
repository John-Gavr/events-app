﻿using Events.Core.Entities;
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

    public async Task<IEnumerable<Event>> GetAllEventsAsync(int pageNumber, int pageSize)
    {
        return await _context.Events
                             .Include(e => e.Participants) 
                             .Skip((pageNumber - 1) * pageSize) 
                             .Take(pageSize)
                             .ToListAsync();
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _context.Events.Include(e => e.Participants)
                                    .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Event?> GetEventByNameAsync(string name)
    {
        return await _context.Events.Include(e => e.Participants)
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
        var eventToDelete = await _context.Events.FindAsync(id);
        if (eventToDelete != null)
        {
            _context.Events.Remove(eventToDelete);
            await _unitOfWork.CompleteAsync();
        }
        else
        {
            throw new NotFoundException(nameof(eventToDelete), id);
        }
    }

    public async Task<IEnumerable<Event>> GetEventsByCriteriaAsync(DateTime? date = null, string? location = null, string? category = null, int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.Events.AsQueryable();

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
        if (eventToUpdate != null)
        {
            eventToUpdate.Image = image;
            await _unitOfWork.CompleteAsync(); 
        }
        else
        {
            throw new NotFoundException(nameof(eventToUpdate), id);
        }
    }
}