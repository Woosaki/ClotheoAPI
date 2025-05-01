namespace ClotheoAPI.Domain.Entities;

public class Listing
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required string PostalCode { get; set; }
    public required string City { get; set; }
    public string? Size { get; set; }
    public string? Condition { get; set; }
    public string? Brand { get; set; }
    public string? Color { get; set; }
    public string? Material { get; set; }
    public bool IsActive { get; set; }
    public DateTime PostDate { get; set; }
    public DateTime? LastModified { get; set; }

    public int CategoryId { get; set; }
    public int UserId { get; set; }

    public Category Category { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<Photo> Photos { get; set; } = null!;
}
