using ClotheoAPI.Domain.Repositories;
using ClotheoAPI.Infrastructure.Data;
using ClotheoAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClotheoAPI.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ClotheoDbContext>(options =>
        {
            options
            .UseNpgsql(connectionString)
            .EnableSensitiveDataLogging();
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
    }
}
