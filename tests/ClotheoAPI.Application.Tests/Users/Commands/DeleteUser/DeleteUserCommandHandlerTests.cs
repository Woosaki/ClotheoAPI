using ClotheoAPI.Application.Auth.Context;
using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClotheoAPI.Application.Users.Commands.DeleteUser.Tests;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userContextMock = new Mock<IUserContext>();
        _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object, _userContextMock.Object);
    }

    [Fact]
    public async Task Handle_CurrentUserIsAdmin_DeletesUser()
    {
        var command = new DeleteUserCommand(id: 2);
        var userToDelete = new User
        {
            Id = 2,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "",
        };
        var currentUser = new CurrentUser(Id: 1, Username: "admin", Email: "admin@example.com", IsAdmin: true);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(userToDelete);

        _userContextMock
            .Setup(context => context.GetCurrentUser())
            .Returns(currentUser);

        _userRepositoryMock
            .Setup(repo => repo.DeleteAsync(userToDelete))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.DeleteAsync(userToDelete), Times.Once);
    }

    [Fact]
    public async Task Handle_CurrentUserIsSelf_DeletesUser()
    {
        var command = new DeleteUserCommand(id: 1);
        var userToDelete = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "",
        };
        var currentUser = new CurrentUser(Id: 1, Username: "testuser", Email: "test@example.com", IsAdmin: false);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(userToDelete);

        _userContextMock
            .Setup(context => context.GetCurrentUser())
            .Returns(currentUser);

        _userRepositoryMock
            .Setup(repo => repo.DeleteAsync(userToDelete))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.DeleteAsync(userToDelete), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingUser_ThrowsNotFoundException()
    {
        var command = new DeleteUserCommand(id: 1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync((User?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User with ID '{command.Id}' was not found.");
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_NoCurrentUser_ThrowsUnauthorizedException()
    {
        var command = new DeleteUserCommand(id: 1);
        var userToDelete = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "",
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(userToDelete);

        _userContextMock
            .Setup(context => context.GetCurrentUser())
            .Returns((CurrentUser?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CurrentUserNotAdminAndNotSelf_ThrowsForbiddenException()
    {
        var command = new DeleteUserCommand(id: 2);
        var userToDelete = new User
        {
            Id = 2,
            Username = "deleteuser",
            Email = "delete@example.com",
            PasswordHash = "",
        };
        var currentUser = new CurrentUser(Id: 1, Username: "testuser", Email: "test@example.com", IsAdmin: false);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(userToDelete);

        _userContextMock
            .Setup(context => context.GetCurrentUser())
            .Returns(currentUser);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>();
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _userRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<User>()), Times.Never);
    }
}