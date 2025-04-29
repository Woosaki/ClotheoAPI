using ClotheoAPI.Application.Users.Commands.DeleteUser;
using ClotheoAPI.Application.Users.Commands.UpdateUser;
using ClotheoAPI.Application.Users.Queries.GetAllUsers;
using ClotheoAPI.Application.Users.Queries.GetUserById;
using ClotheoAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClotheoAPI.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var users = await mediator.Send(new GetAllUsersQuery());

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id));

        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserCommand command)
    {
        command.Id = id;

        await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteUserCommand(id));

        return NoContent();
    }
}
