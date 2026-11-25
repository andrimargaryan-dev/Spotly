using Spotly.Core.Entities;

namespace Spotly.Core.Interfaces;

public interface IVenueRepository
{
    Task<IReadOnlyList<Venue>> GetAllAsync();
    Task<Venue?> GetByIdAsync(int id);
    void Add(Venue venue);
}