using ClotheoAPI.Application.Categories.Commands.AddCategory;
using ClotheoAPI.Application.Categories.Commands.DeleteCategory;
using ClotheoAPI.Application.Categories.Queries.GetAllCategories;
using ClotheoAPI.Application.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClotheoAPI.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await mediator.Send(new GetAllCategoriesQuery());

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await mediator.Send(new GetCategoryByIdQuery(id));

        return Ok(category);
    }

    [HttpPost()]
    public async Task<IActionResult> Add(AddCategoryCommand command)
    {
        var categoryId = await mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { id = categoryId }, null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteCategoryCommand(id));

        return NoContent();
    }
}
