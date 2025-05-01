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

        var adminUser = new User
        {
            Id = 1,
            Username = "admin",
            Email = "admin@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
            IsAdmin = true
        };

        modelBuilder.Entity<User>(e =>
        {
            e.Property(u => u.RegistrationDate).HasDefaultValueSql("now()");
            e.Property(u => u.IsAdmin).HasDefaultValue(false);
            e.HasData(adminUser);
        });

        modelBuilder.Entity<Listing>(e =>
        {
            e.Property(l => l.Price).HasColumnType("numberic(10,2)");
            e.Property(l => l.IsActive).HasDefaultValue(true);
            e.Property(l => l.PostDate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Message>(e =>
        {
            e.Property(m => m.SendDate).HasDefaultValueSql("now()");
        });
    }
}
