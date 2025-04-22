using ClotheoAPI.Domain.Entities;
using MediatR;

namespace ClotheoAPI.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery(int id) : IRequest<Category>
{
    public int Id { get; } = id;
}
