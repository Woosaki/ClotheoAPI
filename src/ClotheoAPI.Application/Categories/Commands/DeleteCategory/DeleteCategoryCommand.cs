using MediatR;

namespace ClotheoAPI.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand(int id) : IRequest
{
    public int Id { get; } = id;
}
