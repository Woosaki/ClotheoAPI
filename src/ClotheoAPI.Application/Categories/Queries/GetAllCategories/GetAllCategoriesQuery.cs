using ClotheoAPI.Domain.Entities;
using MediatR;

namespace ClotheoAPI.Application.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQuery : IRequest<IEnumerable<Category>>
{
}
