using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Repositories;
using ClotheoAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClotheoAPI.Infrastructure.Repositories;

public class UserRepository(ClotheoDbContext dbContext) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAsync()
    {
        return await dbContext.User.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await dbContext.User.FirstOrDefaultAsync(x => x.Id == id);

        return user;
    }

    public async Task<User?> GetByUsernameAsync(string username)
        => await GetByUsernameOrEmailAsync(username, null);

    public async Task<User?> GetByEmailAsync(string email)
        => await GetByUsernameOrEmailAsync(null, email);

    public async Task<User?> GetByUsernameOrEmailAsync(string? username, string? email)
    {
        var user = await dbContext.User.FirstOrDefaultAsync(x => x.Username == username || x.Email == email);
        return user;
    }

    public async Task AddAsync(User user)
    {
        await dbContext.User.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        dbContext.User.Remove(user);
        await dbContext.SaveChangesAsync();
    }
}
