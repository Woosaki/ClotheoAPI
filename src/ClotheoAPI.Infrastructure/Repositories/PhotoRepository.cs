using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Repositories;
using ClotheoAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClotheoAPI.Infrastructure.Repositories;

public class PhotoRepository(ClotheoDbContext dbContext) : IPhotoRepository
{
    public async Task<IEnumerable<Photo>> GetAsync()
    {
        return await dbContext.Photo.ToListAsync();
    }

    public async Task<Photo?> GetByIdAsync(int id)
    {
        var photo = await dbContext.Photo.FirstOrDefaultAsync(x => x.Id == id);

        return photo;
    }

    public async Task AddAsync(Photo photo)
    {
        await dbContext.Photo.AddAsync(photo);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Photo photo)
    {
        dbContext.Photo.Remove(photo);
        await dbContext.SaveChangesAsync();
    }
}
