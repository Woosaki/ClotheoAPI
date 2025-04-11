using ClotheoAPI.Application.Users.Commands.DeleteUser;
using ClotheoAPI.Application.Users.Commands.RegisterUser;
using ClotheoAPI.Application.Users.Commands.UpdateUser;
using ClotheoAPI.Application.Users.Queries.GetAllUsers;
using ClotheoAPI.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClotheoAPI.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await mediator.Send(new GetAllUsersQuery());

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id));

        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var userId = await mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { id = userId }, null);
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
