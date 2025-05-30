﻿using ClotheoAPI.Application.Categories.Commands.AddCategory;
using ClotheoAPI.Application.Categories.Commands.DeleteCategory;
using ClotheoAPI.Application.Categories.Queries.GetAllCategories;
using ClotheoAPI.Application.Categories.Queries.GetCategoryById;
using ClotheoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClotheoAPI.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Category>>> Get()
    {
        var categories = await mediator.Send(new GetAllCategoriesQuery());

        return Ok(categories);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await mediator.Send(new GetCategoryByIdQuery(id));

        return Ok(category);
    }

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(AddCategoryCommand command)
    {
        var categoryId = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = categoryId }, null);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteCategoryCommand(id));

        return NoContent();
    }
}
