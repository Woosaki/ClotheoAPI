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
}
