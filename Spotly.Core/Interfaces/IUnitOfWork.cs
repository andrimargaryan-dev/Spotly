namespace Spotly.Core.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}