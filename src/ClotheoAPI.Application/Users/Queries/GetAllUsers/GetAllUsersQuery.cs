using ClotheoAPI.Domain.Entities;
using MediatR;

namespace ClotheoAPI.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<IEnumerable<User>>
{
}
