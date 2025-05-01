using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Repositories;
using ClotheoAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClotheoAPI.Infrastructure.Repositories;

public class MessageRepository(ClotheoDbContext dbContext) : IMessageRepository
{
    public async Task<IEnumerable<Message>> GetAsync()
    {
        return await dbContext.Message.ToListAsync();
    }

    public async Task<Message?> GetByIdAsync(int id)
    {
        var message = await dbContext.Message.FirstOrDefaultAsync(x => x.Id == id);

        return message;
    }

    public async Task AddAsync(Message message)
    {
        await dbContext.Message.AddAsync(message);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Message message)
    {
        dbContext.Message.Remove(message);
        await dbContext.SaveChangesAsync();
    }
}
