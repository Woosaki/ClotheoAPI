using MediatR;

namespace ClotheoAPI.Application.Auth.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<int>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
}
