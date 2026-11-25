using Microsoft.EntityFrameworkCore;
using Spotly.Core.Entities;
using Spotly.Core.Interfaces;
using Spotly.Infrastructure.Data;

namespace Spotly.Infrastructure.Repositories;

public class CategoryRepository:ICategoryRepository
{
    private readonly AppDbContext _db;

    public CategoryRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync() =>
        await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.SortOrder)
            .ToListAsync();

    public Task<Category?> GetBySlugAsync(string slug) =>
        _db.Categories
            .AsNoTracking()
            .Include(c => c.Parent)
            .FirstOrDefaultAsync(c => c.Slug == slug);
}