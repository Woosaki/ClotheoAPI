using MediatR;

namespace ClotheoAPI.Application.Auth.Commands.LoginUser;

public class LoginUserCommand : IRequest<string>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
