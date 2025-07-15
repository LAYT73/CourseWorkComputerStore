using ComputerStoreModels.Models;

namespace ComputerStoreContracts.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByLoginAsync(string login);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<bool> ValidateUserAsync(string login, string password);
}