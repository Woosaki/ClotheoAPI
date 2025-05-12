using FluentValidation;

namespace ClotheoAPI.Application.Auth.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").WithMessage("Email is not a valid email address.");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
