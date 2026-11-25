using Spotly.Core.Entities;

namespace Spotly.Core.Interfaces;

public interface IVenueRepository
{
    Task<IReadOnlyList<Venue>> GetAllAsync();
    Task<Venue?> GetBySlugAsync(string slug);
    void Add(Venue venue);
}