using ClotheoAPI.Application.Auth.Context;
using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using MediatR;

namespace ClotheoAPI.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository, IUserContext userContext)
    : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userToUpdate = await userRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(User), request.Id);

        var currentUser = userContext.GetCurrentUser()
            ?? throw new UnauthorizedException();

        if (currentUser.Id != userToUpdate.Id && !currentUser.IsAdmin)
        {
            throw new ForbiddenException();
        }

        if (request.Username is not null)
        {
            userToUpdate.Username = request.Username;
        }
        if (request.Password is not null)
        {
            userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }
        if (request.ProfilePicture is not null)
        {
            userToUpdate.ProfilePicture = request.ProfilePicture;
        }

        await userRepository.UpdateAsync(userToUpdate);
    }
}
