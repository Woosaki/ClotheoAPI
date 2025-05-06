using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using ClotheoAPI.Domain.Repositories;
using MediatR;

namespace ClotheoAPI.Application.Categories.Commands.AddCategory;

public class AddCategoryCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<AddCategoryCommand, int>
{
    public async Task<int> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var formattedName = FormatCategoryName(request.Name);

        var existingCategory = await categoryRepository.GetByNameAsync(formattedName);
        if (existingCategory != null)
        {
            throw new BadRequestException($"Category '{formattedName}' already exists.");
        }

        var newCategory = new Category { Name = formattedName };
        await categoryRepository.AddAsync(newCategory);

        return newCategory.Id;
    }

    private static string FormatCategoryName(string name)
    {
        string firstLetterUpper = name[..1].ToUpperInvariant();
        string restToLower = name[1..].ToLowerInvariant();

        return firstLetterUpper + restToLower;
    }
}
