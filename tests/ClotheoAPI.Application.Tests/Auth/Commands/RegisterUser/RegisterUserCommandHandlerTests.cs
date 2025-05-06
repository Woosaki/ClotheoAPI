using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClotheoAPI.Application.Auth.Commands.RegisterUser.Tests;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new RegisterUserCommandHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidNewUser_AddsUserToRepositoryAndReturnsId()
    {
        var command = new RegisterUserCommand
        {
            Username = "newuser",
            Email = "newuser@example.com",
            Password = "password123",
            ProfilePicture = "profile.jpg"
        };
        var newUser = new User
        {
            Id = 1,
            Username = command.Username,
            Email = command.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password),
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByUsernameOrEmailAsync(command.Username, command.Email))
            .ReturnsAsync((User?)null);

        _userRepositoryMock
            .Setup(repo => repo.AddAsync(It.Is<User>(u =>
                u.Username == command.Username &&
                u.Email == command.Email &&
                !string.IsNullOrEmpty(u.PasswordHash) &&
                u.ProfilePicture == command.ProfilePicture)))
            .Returns(Task.CompletedTask)
            .Callback<User>(u => u.Id = newUser.Id);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(newUser.Id);
        _userRepositoryMock.Verify(repo => repo.GetByUsernameOrEmailAsync(command.Username, command.Email), Times.Once);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.Is<User>(u =>
                u.Username == command.Username &&
                u.Email == command.Email &&
                !string.IsNullOrEmpty(u.PasswordHash) &&
                u.ProfilePicture == command.ProfilePicture)), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingUsername_ThrowsBadRequestException()
    {
        var command = new RegisterUserCommand
        {
            Username = "testUser",
            Email = "test@example.com",
            Password = ""
        };
        var existingUser = new User
        {
            Id = 1,
            Username = command.Username,
            Email = "",
            PasswordHash = ""
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByUsernameOrEmailAsync(command.Username, command.Email))
            .ReturnsAsync(existingUser);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Username or email already exists.");
        _userRepositoryMock.Verify(repo => repo.GetByUsernameOrEmailAsync(command.Username, command.Email), Times.Once);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ExistingEmail_ThrowsBadRequestException()
    {
        var command = new RegisterUserCommand
        {
            Username = "Test",
            Email = "test@example.com",
            Password = ""
        };
        var existingUser = new User
        {
            Id = 1,
            Username = "",
            Email = command.Email,
            PasswordHash = ""
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByUsernameOrEmailAsync(command.Username, command.Email))
            .ReturnsAsync(existingUser);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Username or email already exists.");
        _userRepositoryMock.Verify(repo => repo.GetByUsernameOrEmailAsync(command.Username, command.Email), Times.Once);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
    }
}