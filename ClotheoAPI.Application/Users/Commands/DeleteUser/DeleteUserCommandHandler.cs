using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using MediatR;

namespace ClotheoAPI.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userToDelete = await userRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(User), request.Id);

        await userRepository.DeleteAsync(userToDelete);
    }
}
