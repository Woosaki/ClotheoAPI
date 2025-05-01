using MediatR;

namespace ClotheoAPI.Application.Categories.Commands.AddCategory;

public class AddCategoryCommand : IRequest<int>
{
    public required string Name { get; set; }
}
