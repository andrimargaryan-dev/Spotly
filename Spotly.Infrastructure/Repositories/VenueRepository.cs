using Microsoft.EntityFrameworkCore;
using Spotly.Core.Entities;
using Spotly.Core.Interfaces;
using Spotly.Infrastructure.Data;

namespace Spotly.Infrastructure.Repositories;

public class VenueRepository : IVenueRepository
{
    private readonly AppDbContext _db;

    public VenueRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Venue>> GetAllAsync() =>
        await _db.Venues
            .AsNoTracking()
            .OrderBy(v => v.Name)
            .ToListAsync();

    public async Task<Venue?> GetByIdAsync(int id) =>
        await _db.Venues.FindAsync(id);

    public void Add(Venue venue) => _db.Venues.Add(venue);
}