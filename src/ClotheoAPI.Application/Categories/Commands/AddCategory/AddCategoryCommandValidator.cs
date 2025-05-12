using FluentValidation;

namespace ClotheoAPI.Application.Categories.Commands.AddCategory;

public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .Length(3, 50).WithMessage("Category name must be between 3 and 50 characters.");
    }
}
