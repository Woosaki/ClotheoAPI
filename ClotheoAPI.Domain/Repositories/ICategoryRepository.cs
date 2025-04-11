using ClotheoAPI.Domain.Entities;

namespace ClotheoAPI.Domain.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAsync();
    Task<Category?> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task DeleteAsync(Category category);
}
