using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Repositories;
using ClotheoAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClotheoAPI.Infrastructure.Repositories;

public class CategoryRepository(ClotheoDbContext dbContext) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAsync()
    {
        return await dbContext.Category.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        var category = await dbContext.Category.FirstOrDefaultAsync(x => x.Id == id);

        return category;
    }
    public async Task AddAsync(Category category)
    {
        await dbContext.Category.AddAsync(category);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        dbContext.Category.Remove(category);
        await dbContext.SaveChangesAsync();
    }
}
