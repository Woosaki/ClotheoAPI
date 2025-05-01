using MediatR;
using System.Text.Json.Serialization;

namespace ClotheoAPI.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ProfilePicture { get; set; }
}
