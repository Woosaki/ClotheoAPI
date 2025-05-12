using FluentValidation.TestHelper;
using Xunit;

namespace ClotheoAPI.Application.Auth.Commands.RegisterUser.Tests;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator;

    public RegisterUserCommandValidatorTests()
    {
        _validator = new RegisterUserCommandValidator();
    }

    [Fact]
    public void Register_Should_not_have_errors_when_all_fields_are_valid()
    {
        var command = new RegisterUserCommand
        {
            Username = "validUser",
            Email = "valid@email.com",
            Password = "Password123!"
        };
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Register_Should_have_error_when_username_is_empty()
    {
        var command = new RegisterUserCommand
        {
            Username = "",
            Email = "test@example.com",
            Password = "Password123!"
        };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Username)
            .WithErrorMessage("Username is required.");
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("ThisUsernameIsTooLongAndExceedsTheMaximumAllowedLengthOf50Characters")]
    public void Register_Should_have_error_when_username_length_is_invalid(string username)
    {
        var command = new RegisterUserCommand
        {
            Username = username,
            Email = "test@example.com",
            Password = "Password123!"
        };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Username)
            .WithErrorMessage("Username must be between 3 and 50 characters.");
    }

    [Fact]
    public void Register_Should_have_error_when_email_is_empty()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
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
    public void Register_Should_have_error_when_email_is_not_a_valid_email_address(string email)
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = email,
            Password = "Password123!"
        };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is not a valid email address.");
    }

    [Fact]
    public void Register_Should_have_error_when_password_is_empty()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = ""
        };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }

    [Fact]
    public void Register_Should_have_error_when_password_is_too_short()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Short!"
        };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 8 characters long.");
    }

    [Theory]
    [InlineData("lowercase")]
    [InlineData("UPPERCASE")]
    [InlineData("12345678")]
    [InlineData("********")]
    [InlineData("Upper123")]
    [InlineData("lower123")]
    [InlineData("lowerUpper")]
    [InlineData("123****")]
    [InlineData("Upper****")]
    [InlineData("lower****")]
    public void Register_Should_have_error_when_password_does_not_meet_complexity_requirements(string password)
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = password
        };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one non-alphanumeric character.");
    }
}