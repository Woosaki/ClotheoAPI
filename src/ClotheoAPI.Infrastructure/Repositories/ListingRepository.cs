using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Repositories;
using ClotheoAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClotheoAPI.Infrastructure.Repositories;

public class ListingRepository(ClotheoDbContext dbContext) : IListingRepository
{
    public async Task<IEnumerable<Listing>> GetAsync()
    {
        return await dbContext.Listing.ToListAsync();
    }

    public async Task<Listing?> GetByIdAsync(int id)
    {
        var listing = await dbContext.Listing.FirstOrDefaultAsync(x => x.Id == id);

        return listing;
    }

    public async Task AddAsync(Listing listing)
    {
        await dbContext.Listing.AddAsync(listing);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Listing listing)
    {
        dbContext.Listing.Remove(listing);
        await dbContext.SaveChangesAsync();
    }
}