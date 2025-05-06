using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClotheoAPI.Application.Categories.Commands.AddCategory.Tests;

public class AddCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly AddCategoryCommandHandler _handler;

    public AddCategoryCommandHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new AddCategoryCommandHandler(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidNewCategory_ReturnsCategoryId()
    {
        var command = new AddCategoryCommand { Name = "test" };
        var expectedFormattedName = "Test";
        var newCategory = new Category { Id = 1, Name = expectedFormattedName };

        _categoryRepositoryMock
            .Setup(repo => repo.GetByNameAsync(expectedFormattedName))
            .ReturnsAsync((Category?)null);

        _categoryRepositoryMock
            .Setup(repo => repo.AddAsync(It.Is<Category>(c => c.Name == expectedFormattedName)))
            .Callback<Category>(c => c.Id = newCategory.Id);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(newCategory.Id);
        _categoryRepositoryMock.Verify(repo => repo.GetByNameAsync(expectedFormattedName), Times.Once);
        _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Category>(c => c.Name == expectedFormattedName)), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingCategory_ThrowsBadRequestException()
    {
        var command = new AddCategoryCommand { Name = "test" };
        var existingCategory = new Category { Id = 1, Name = "Test" };

        _categoryRepositoryMock
            .Setup(repo => repo.GetByNameAsync(existingCategory.Name))
            .ReturnsAsync(existingCategory);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage($"Category '{existingCategory.Name}' already exists.");
    }

    [Theory]
    [InlineData("dress", "Dress")]
    [InlineData("DRESS", "Dress")]
    [InlineData("dRess", "Dress")]
    [InlineData("dResS", "Dress")]
    public async Task Handle_ValidNewCategoryWithVaryingCase_AddsCategoryWithCorrectlyFormattedName
        (string inputName, string expectedFormattedName)
    {
        var command = new AddCategoryCommand { Name = inputName };
        var newCategory = new Category { Id = 1, Name = expectedFormattedName };

        _categoryRepositoryMock
            .Setup(repo => repo.GetByNameAsync(expectedFormattedName))
            .ReturnsAsync((Category?)null);

        _categoryRepositoryMock
            .Setup(repo => repo.AddAsync(It.Is<Category>(c => c.Name == expectedFormattedName)))
            .Callback<Category>(c => c.Id = newCategory.Id);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(newCategory.Id);
        _categoryRepositoryMock.Verify(repo => repo.GetByNameAsync(expectedFormattedName), Times.Once);
        _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Category>(c => c.Name == expectedFormattedName)), Times.Once);
    }
}
