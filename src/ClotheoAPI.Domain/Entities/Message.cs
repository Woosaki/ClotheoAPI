namespace ClotheoAPI.Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public DateTime SendDate { get; set; }

    public int? SenderId { get; set; }
    public int? ReceiverId { get; set; }

    public User Sender { get; set; } = null!;
    public User Receiver { get; set; } = null!;
}
