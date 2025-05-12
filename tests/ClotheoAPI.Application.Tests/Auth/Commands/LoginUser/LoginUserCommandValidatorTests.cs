using FluentValidation.TestHelper;
using Xunit;

namespace ClotheoAPI.Application.Auth.Commands.LoginUser.Tests;

public class LoginUserCommandValidatorTests
{
    private readonly LoginUserCommandValidator _validator;

    public LoginUserCommandValidatorTests()
    {
        _validator = new LoginUserCommandValidator();
    }

    [Fact]
    public void Login_Should_not_have_errors_when_all_fields_are_valid()
    {
        var command = new LoginUserCommand
        {
            Email = "valid@example.com",
            Password = "Password123!"
        };
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Login_Should_have_error_when_email_is_empty()
    {
        var command = new LoginUserCommand
        {
            Email = "",
            Password = "Password123!"
        };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required.");
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("test@test")]
    [InlineData("test@.com")]
    [InlineData("@example.com")]
    public void Login_Should_have_error_when_email_is_not_a_valid_email_address(string email)
    {
        var command = new LoginUserCommand
        {
            Email = email,
            Password = "Password123!"
        };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is not a valid email address.");
    }

    [Fact]
    public void Login_Should_have_error_when_password_is_empty()
    {
        var command = new LoginUserCommand
        {
            Email = "valid@example.com",
            Password = ""
        };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }
}