using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClotheoAPI.Application.Categories.Queries.GetCategoryById.Tests;

public class GetCategoryByIdQueryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly GetCategoryByIdQueryHandler _handler;

    public GetCategoryByIdQueryHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new GetCategoryByIdQueryHandler(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingCategory_ReturnsCategory()
    {
        var query = new GetCategoryByIdQuery(id: 1);
        var expectedCategory = new Category { Id = 1, Name = "Test" };

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(query.Id))
            .ReturnsAsync(expectedCategory);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedCategory);
        _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(query.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCategory_ThrowsNotFoundException()
    {
        var query = new GetCategoryByIdQuery(id: 1);

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(query.Id))
            .ReturnsAsync((Category?)null);

        var act = async () => await _handler.Handle(query, CancellationToken.None);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User with ID '{query.Id}' was not found.");
        _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(query.Id), Times.Once);
    }
}