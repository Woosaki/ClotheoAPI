using ClotheoAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClotheoAPI.Infrastructure.Data;

public class ClotheoDbContext(DbContextOptions<ClotheoDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<Listing> Listing { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<Photo> Photo { get; set; }
    public DbSet<Category> Category { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Listing>(e =>
        {
            e.Property(l => l.Price).HasColumnType("numberic(10,2)");
            e.Property(l => l.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<User>(e =>
        {
            e.Property(u => u.IsAdmin).HasDefaultValue(false);
        });
    }
}
