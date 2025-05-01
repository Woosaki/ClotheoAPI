using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClotheoAPI.Infrastructure.Data;

public class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, ILogger logger)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ClotheoDbContext>();

        await ApplyPendingMigrationsAsync(dbContext, logger);
        await CheckDatabaseConnectionAsync(dbContext, logger);
    }

    private static async Task ApplyPendingMigrationsAsync(ClotheoDbContext dbContext, ILogger logger)
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation($"Found {pendingMigrations.Count()} pending migrations. Applying...");
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Migrations applied successfully.");
        }
        else
        {
            logger.LogInformation("No pending migrations to apply.");
        }
    }

    private static async Task CheckDatabaseConnectionAsync(ClotheoDbContext dbContext, ILogger logger)
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            logger.LogInformation("Database connection successful.");
        }
        else
        {
            logger.LogError("Database connection failed.");
            throw new Exception("Database connection failed.");
        }
    }
}
