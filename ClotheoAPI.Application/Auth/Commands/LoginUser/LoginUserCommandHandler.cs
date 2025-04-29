using MediatR;

namespace ClotheoAPI.Application.Auth.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand>
{
    public async Task Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
