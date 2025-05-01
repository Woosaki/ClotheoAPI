using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using MediatR;

namespace ClotheoAPI.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, Category>
{
    public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(User), request.Id);

        return category;
    }
}
