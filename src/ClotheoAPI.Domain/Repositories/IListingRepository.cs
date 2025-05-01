using ClotheoAPI.Domain.Entities;

namespace ClotheoAPI.Domain.Repositories;

public interface IListingRepository
{
    Task<IEnumerable<Listing>> GetAsync();
    Task<Listing?> GetByIdAsync(int id);
    Task AddAsync(Listing listing);
    Task DeleteAsync(Listing listing);
}
