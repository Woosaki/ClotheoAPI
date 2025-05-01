using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Repositories;
using MediatR;

namespace ClotheoAPI.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetAsync();
    }
}
