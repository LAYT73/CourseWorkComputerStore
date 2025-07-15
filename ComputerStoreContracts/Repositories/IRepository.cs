using ComputerStoreModels.Models;

namespace ComputerStoreContracts.Repositories;

public interface IRepository<T> where T : class, IUserOwnedEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllByUserIdAsync(Guid userId);

    Task AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(Guid id);

    Task<bool> SaveChangesAsync();
}
