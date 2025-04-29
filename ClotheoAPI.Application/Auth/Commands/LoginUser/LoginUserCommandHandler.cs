using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ClotheoAPI.Application.Auth.Commands.LoginUser;

public class LoginUserCommandHandler(IUserRepository userRepository) : IRequestHandler<LoginUserCommand>
{
    public async Task Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user?.PasswordHash);

        if (user is null || !isPasswordValid)
        {
            throw new BadRequestException("Wrong login credentials");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes())
    }
}
