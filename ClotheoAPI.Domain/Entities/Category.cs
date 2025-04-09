namespace ClotheoAPI.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Listing> Listings { get; set; } = null!;
}
