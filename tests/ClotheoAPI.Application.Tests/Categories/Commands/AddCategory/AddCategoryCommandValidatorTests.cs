using FluentValidation.TestHelper;
using Xunit;

namespace ClotheoAPI.Application.Categories.Commands.AddCategory.Tests;

public class AddCategoryCommandValidatorTests
{
    private readonly AddCategoryCommandValidator _validator;

    public AddCategoryCommandValidatorTests()
    {
        _validator = new AddCategoryCommandValidator();
    }

    [Fact]
    public void AddCategory_Should_not_have_errors_when_name_is_valid()
    {
        var command = new AddCategoryCommand { Name = "t-shirt" };
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void AddCategory_Should_have_error_when_name_is_empty()
    {
        var command = new AddCategoryCommand { Name = "" };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Category name is required.");
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("ThisCategoryNameIsTooLongAndExceedsMaximumAllowedLength")]
    public void AddCategory_Should_have_error_when_name_length_is_invalid(string name)
    {
        var command = new AddCategoryCommand { Name = name };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Category name must be between 3 and 50 characters.");
    }
}