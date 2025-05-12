using MediatR;

namespace ClotheoAPI.Application.Auth.Commands.LoginUser;

public class LoginUserCommand : IRequest<string>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
