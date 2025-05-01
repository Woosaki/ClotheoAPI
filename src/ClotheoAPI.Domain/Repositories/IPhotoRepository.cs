using ClotheoAPI.Domain.Entities;

namespace ClotheoAPI.Domain.Repositories;

public interface IPhotoRepository
{
    Task<IEnumerable<Photo>> GetAsync();
    Task<Photo?> GetByIdAsync(int id);
    Task AddAsync(Photo photo);
    Task DeleteAsync(Photo photo);
}
