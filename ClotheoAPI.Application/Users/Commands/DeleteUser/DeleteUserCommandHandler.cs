using ClotheoAPI.Application.Auth.Context;
using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using MediatR;

namespace ClotheoAPI.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository, IUserContext userContext)
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userToDelete = await userRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(User), request.Id);

        var currentUser = userContext.GetCurrentUser()
            ?? throw new UnauthorizedException();

        if (currentUser.Id != userToDelete.Id && !currentUser.IsAdmin)
        {
            throw new ForbiddenException();
        }

        await userRepository.DeleteAsync(userToDelete);
    }
}
