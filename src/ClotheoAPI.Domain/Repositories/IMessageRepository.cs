using ClotheoAPI.Domain.Entities;

namespace ClotheoAPI.Domain.Repositories;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetAsync();
    Task<Message?> GetByIdAsync(int id);
    Task AddAsync(Message message);
    Task DeleteAsync(Message message);
}
