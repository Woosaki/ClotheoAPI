using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClotheoAPI.Application.Users.Queries.GetUserById.Tests;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingUser_ReturnsUser()
    {
        var query = new GetUserByIdQuery(id: 1);
        var expectedUser = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "",
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(query.Id))
            .ReturnsAsync(expectedUser);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedUser);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(query.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCategory_ThrowsNotFoundException()
    {
        var query = new GetUserByIdQuery(id: 1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(query.Id))
            .ReturnsAsync((User?)null);

        var act = async () => await _handler.Handle(query, CancellationToken.None);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User with ID '{query.Id}' was not found.");
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(query.Id), Times.Once);
    }
}