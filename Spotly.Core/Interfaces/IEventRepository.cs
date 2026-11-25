using Spotly.Core.Entities;

namespace Spotly.Core.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<Event>> GetUpcomingAsync(int count);
    Task<bool> SlugExistsAsync(string slug);
    void Add(Event ev);
}