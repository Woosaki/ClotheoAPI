using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using MediatR;

namespace ClotheoAPI.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryToDelete = await categoryRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Category), request.Id);

        await categoryRepository.DeleteAsync(categoryToDelete);
    }
}
