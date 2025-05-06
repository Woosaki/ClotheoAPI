using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClotheoAPI.Application.Categories.Commands.DeleteCategory.Tests;

public class DeleteCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly DeleteCategoryCommandHandler _handler;

    public DeleteCategoryCommandHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new DeleteCategoryCommandHandler(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingCategory_DeletesCategory()
    {
        var command = new DeleteCategoryCommand(id: 1);
        var existingCategory = new Category { Id = 1, Name = "Test" };

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(existingCategory);

        _categoryRepositoryMock
            .Setup(repo => repo.DeleteAsync(existingCategory))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _categoryRepositoryMock.Verify(repo => repo.DeleteAsync(existingCategory), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCategory_ThrowsNotFoundException()
    {
        var command = new DeleteCategoryCommand(id: 1);

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync((Category?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category with ID '{command.Id}' was not found.");
        _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _categoryRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Category>()), Times.Never);
    }
}