using MediatR;

namespace ClotheoAPI.Application.Auth.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<int>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? ProfilePicture { get; set; }
}
