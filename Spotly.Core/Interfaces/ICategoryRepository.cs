using Spotly.Core.Entities;

namespace Spotly.Core.Interfaces;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync();
    Task<Category?> GetBySlugAsync(string slug);
}