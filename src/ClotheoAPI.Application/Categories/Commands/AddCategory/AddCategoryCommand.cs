using MediatR;

namespace ClotheoAPI.Application.Categories.Commands.AddCategory;

public class AddCategoryCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
}
