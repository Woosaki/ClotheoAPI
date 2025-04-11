using ClotheoAPI.Domain.Entities;
using MediatR;

namespace ClotheoAPI.Application.Users.Queries.GetUserById;

public class GetUserByIdQuery(int id) : IRequest<User>
{
    public int Id { get; } = id;
}
