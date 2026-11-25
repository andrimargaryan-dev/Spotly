using Microsoft.EntityFrameworkCore;
using Spotly.Core.Entities;
using Spotly.Core.Enums;
using Spotly.Core.Interfaces;
using Spotly.Infrastructure.Data;

namespace Spotly.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _db;

    public EventRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Event?> GetBySlugAsync(string slug) =>
        _db.Events
            .AsNoTracking()
            .Include(e => e.Category).ThenInclude(c => c.Parent)
            .Include(e => e.Venue)
            .Include(e => e.Sessions)
            .Include(e => e.TicketTypes)
            .AsSplitQuery()
            .FirstOrDefaultAsync(e => e.Slug == slug);

    public async Task<IReadOnlyList<Event>> GetUpcomingAsync(int count)
    {
        var now = DateTime.UtcNow;

        return await _db.Events
            .AsNoTracking()
            .Where(e => e.Status == EventStatus.Published
                        && e.Sessions.Any(s => !s.IsCanceled && s.StartsAt > now))
            .OrderBy(e => e.Sessions
                .Where(s => !s.IsCanceled && s.StartsAt > now)
                .Min(s => s.StartsAt))
            .Take(count)
            .Include(e => e.Category)
            .Include(e => e.Venue)
            .Include(e => e.Sessions)
            .Include(e => e.TicketTypes)
            .AsSplitQuery()
            .ToListAsync();
    }

    public Task<bool> SlugExistsAsync(string slug) =>
        _db.Events.AnyAsync(e => e.Slug == slug);

    public void Add(Event ev) => _db.Events.Add(ev);
}