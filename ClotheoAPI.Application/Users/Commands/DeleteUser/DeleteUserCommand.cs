using MediatR;

namespace ClotheoAPI.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand(int id) : IRequest
{
    public int Id { get; } = id;
}
