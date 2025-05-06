using ClotheoAPI.Application.Auth.Context;
using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClotheoAPI.Application.Users.Commands.UpdateUser.Tests;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userContextMock = new Mock<IUserContext>();
        _handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _userContextMock.Object);
    }

    [Fact]
    public async Task Handle_CurrentUserIsAdmin_UpdatesUser()
    {
        var command = new UpdateUserCommand()
        {
            Id = 2,
            Username = "newusername",
            Password = "newpassword",
            ProfilePicture = "newpic.jpg"
        };
        var existingUser = new User
        {
            Id = 2,
            Username = "olduser",
            Email = "old@example.com",
            PasswordHash = "oldhash",
            ProfilePicture = "oldpic.jpg"
        };
        var currentUser = new CurrentUser(Id: 1, Username: "admin", Email: "admin@example.com", IsAdmin: true);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(existingUser);

        _userContextMock
            .Setup(context => context.GetCurrentUser())
            .Returns(currentUser);

        _userRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.Is<User>(u =>
                u.Id == command.Id &&
                u.Username == command.Username &&
                !string.IsNullOrEmpty(u.PasswordHash) &&
                u.ProfilePicture == command.ProfilePicture)))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<User>(u =>
            u.Id == command.Id &&
            u.Username == command.Username &&
            !string.IsNullOrEmpty(u.PasswordHash) &&
            u.ProfilePicture == command.ProfilePicture
        )), Times.Once);
    }

    [Fact]
    public async Task Handle_CurrentUserIsSelf_UpdatesUser()
    {
        var command = new UpdateUserCommand()
        {
            Id = 1,
            Username = "newusername",
            Password = "newpassword",
            ProfilePicture = "newpic.jpg"
        };
        var existingUser = new User
        {
            Id = 1,
            Username = "olduser",
            Email = "old@example.com",
            PasswordHash = "oldhash",
            ProfilePicture = "oldpic.jpg"
        };
        var currentUser = new CurrentUser(Id: 1, Username: "olduser", Email: "old@example.com", IsAdmin: false);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(existingUser);

        _userContextMock
            .Setup(context => context.GetCurrentUser())
            .Returns(currentUser);

        _userRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.Is<User>(u =>
                u.Id == command.Id &&
                u.Username == command.Username &&
                !string.IsNullOrEmpty(u.PasswordHash) &&
                u.ProfilePicture == command.ProfilePicture)))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<User>(u =>
            u.Id == command.Id &&
            u.Username == command.Username &&
            !string.IsNullOrEmpty(u.PasswordHash) &&
            u.ProfilePicture == command.ProfilePicture
        )), Times.Once);
    }

    [Fact]
    public async Task Handle_CurrentUserIsSelf_UpdatesOnlyNonNullProperties()
    {
        var command = new UpdateUserCommand()
        {
            Id = 1,
            Username = "newusername"
        };
        var existingUser = new User
        {
            Id = 1,
            Username = "olduser",
            Email = "old@example.com",
            PasswordHash = "oldhash",
            ProfilePicture = "oldpic.jpg"
        };
        var currentUser = new CurrentUser(Id: 1, Username: "olduser", Email: "old@example.com", IsAdmin: false);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(existingUser);

        _userContextMock
            .Setup(context => context.GetCurrentUser())
            .Returns(currentUser);

        _userRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.Is<User>(u =>
                u.Id == command.Id &&
                u.Username == command.Username &&
                u.Email == "old@example.com" &&
                u.PasswordHash == "oldhash" &&
                u.ProfilePicture == "oldpic.jpg")))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<User>(u =>
            u.Id == command.Id &&
            u.Username == command.Username &&
            u.Email == "old@example.com" &&
            u.PasswordHash == "oldhash" &&
            u.ProfilePicture == "oldpic.jpg"
        )), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingUser_ThrowsNotFoundException()
    {
        var command = new UpdateUserCommand { Id = 1 };

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync((User?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User with ID '{command.Id}' was not found.");
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_NoCurrentUser_ThrowsUnauthorizedException()
    {
        var command = new UpdateUserCommand { Id = 1 };
        var userToUpdate = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "",
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(userToUpdate);

        _userContextMock
            .Setup(context => context.GetCurrentUser())
            .Returns((CurrentUser?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CurrentUserNotAdminAndNotSelf_ThrowsForbiddenException()
    {
        var command = new UpdateUserCommand() { Id = 2 };
        var userToUpdate = new User
        {
            Id = 2,
            Username = "othertestuser",
            Email = "other@example.com",
            PasswordHash = "",
        };
        var currentUser = new CurrentUser(Id: 1, Username: "testuser", Email: "test@example.com", IsAdmin: false);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(userToUpdate);

        _userContextMock
            .Setup(context => context.GetCurrentUser())
            .Returns(currentUser);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>();
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
    }
}