using ComputerStoreContracts.Repositories;
using ComputerStoreModels.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreDatabase.Implementations;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _context.Users.FindAsync(id);

    public async Task<User?> GetByLoginAsync(string login) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

    public async Task CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ValidateUserAsync(string login, string password)
    {
        var user = await GetByLoginAsync(login);
        return user != null && user.Password == password;
    }
}
