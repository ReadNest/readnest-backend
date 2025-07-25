﻿using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class EventRepository(AppDbContext context) : GenericRepository<Event, Guid>(context), IEventRepository
    {
        public async Task<Event?> GetCurrentEventAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Events
                .Where(e => !e.IsDeleted && e.StartDate <= now && e.EndDate >= now)
                .OrderByDescending(e => e.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Where(e => e.IsDeleted == false && e.Status != "Upcoming")
                .ToListAsync();
        }
    }
}
