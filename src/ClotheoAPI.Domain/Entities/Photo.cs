namespace ClotheoAPI.Domain.Entities;

public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }

    public int ListingId { get; set; }

    public Listing Listing { get; set; } = null!;
}
