using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Repositories;
using MediatR;

namespace ClotheoAPI.Application.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
{
    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await categoryRepository.GetAsync();
    }
}
