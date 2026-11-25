using Spotly.Core.Interfaces;
using Spotly.Infrastructure.Data;

namespace Spotly.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
    }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}