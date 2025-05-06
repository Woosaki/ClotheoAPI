using ClotheoAPI.Application.Auth.Settings;
using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using FluentAssertions;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace ClotheoAPI.Application.Auth.Commands.LoginUser.Tests;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly JwtSettings _jwtSettings;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtSettings = new JwtSettings
        {
            SecretKey = "test-secret-key-that-is-at-least-16-bytes",
            Issuer = "test-issuer",
            Audience = "test-audience",
            ExpirationInMinutes = 30
        };
        _handler = new LoginUserCommandHandler(_userRepositoryMock.Object, _jwtSettings);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsCorrectJwtToken()
    {
        var command = new LoginUserCommand
        {
            Email = "test@example.com",
            Password = "password123"
        };
        var existingUser = new User
        {
            Id = 1,
            Username = "testuser",
            Email = command.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password),
            IsAdmin = false
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNullOrEmpty();
        var token = new JwtSecurityTokenHandler().ReadJwtToken(result);
        token.Should().NotBeNull();
        token.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes), TimeSpan.FromSeconds(5));
        token.Issuer.Should().Be(_jwtSettings.Issuer);
        token.Audiences.Should().Contain(_jwtSettings.Audience);
        token.Claims.Should().Contain(c => c.Type == "nameid" && c.Value == existingUser.Id.ToString());
        token.Claims.Should().Contain(c => c.Type == "unique_name" && c.Value == existingUser.Username);
        token.Claims.Should().Contain(c => c.Type == "email" && c.Value == existingUser.Email);
        token.Claims.Should().Contain(c => c.Type == "role" && c.Value == (existingUser.IsAdmin ? "Admin" : "User"));

        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(command.Email), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidEmail_ThrowsBadRequestException()
    {
        var command = new LoginUserCommand
        {
            Email = "nonexistent@example.com",
            Password = "password123"
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage($"User with email '{command.Email}' not found.");
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(command.Email), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidPassword_ThrowsBadRequestException()
    {
        var command = new LoginUserCommand
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };
        var existingUser = new User
        {
            Id = 1,
            Username = "testuser",
            Email = command.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Invalid password.");
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(command.Email), Times.Once);
    }
}