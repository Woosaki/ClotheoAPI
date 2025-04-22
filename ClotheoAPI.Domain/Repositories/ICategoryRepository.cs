using ClotheoAPI.Domain.Entities;

namespace ClotheoAPI.Domain.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category?> GetByNameAsync(string name);
    Task AddAsync(Category category);
    Task DeleteAsync(Category category);
}
