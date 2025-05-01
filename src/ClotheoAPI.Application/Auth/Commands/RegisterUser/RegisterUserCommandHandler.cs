using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using MediatR;

namespace ClotheoAPI.Application.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<RegisterUserCommand, int>
{
    public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByUsernameOrEmailAsync(request.Username, request.Email);
        if (existingUser != null)
        {
            throw new BadRequestException("Username or email already exists.");
        }

        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            ProfilePicture = request.ProfilePicture
        };

        await userRepository.AddAsync(newUser);

        return newUser.Id;
    }
}
