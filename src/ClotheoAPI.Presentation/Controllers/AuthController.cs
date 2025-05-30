﻿using ClotheoAPI.Application.Auth.Commands.LoginUser;
using ClotheoAPI.Application.Auth.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClotheoAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var userId = await mediator.Send(command);

        return CreatedAtAction(nameof(UserController.GetById), new { controller = "user", id = userId }, null);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login(LoginUserCommand command)
    {
        var token = await mediator.Send(command);

        return Ok(token);
    }
}
