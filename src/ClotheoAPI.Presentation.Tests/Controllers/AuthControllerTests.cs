using ClotheoAPI.Application.Auth.Commands.RegisterUser;
using ClotheoAPI.Domain.Common;
using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Infrastructure.Data;
using ClotheoAPI.Presentation.Tests.Factories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ClotheoAPI.Presentation.Controllers.Tests;

public class AuthControllerTests(CustomWebApplicationFactory factory)
        : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly IServiceScopeFactory _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();

    private async Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ClotheoDbContext>();
        return await dbContext.User.SingleOrDefaultAsync(predicate);
    }

    [Fact]
    public async Task Register_ValidRequest_Returns201CreatedAtAction()
    {
        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.ToString().Should().Contain("/api/user/");

        var registeredUser = await GetUserAsync(u => u.Email == command.Email);
        registeredUser.Should().NotBeNull();
        registeredUser.Username.Should().Be(command.Username);
        registeredUser.Email.Should().Be(command.Email);
        registeredUser.PasswordHash.Should().NotBeNullOrEmpty();
        registeredUser.ProfilePicture.Should().BeNull();
        registeredUser.RegistrationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        registeredUser.LastOnlineDate.Should().BeNull();
        registeredUser.IsAdmin.Should().BeFalse();
    }

    [Fact]
    public async Task Register_ExistingUser_Returns400BadRequest_WithErrorMessage()
    {
        var command = new RegisterUserCommand
        {
            Username = "existinguser",
            Email = "existing@email.com",
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        errorResponse.Should().NotBeNull();
        errorResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        errorResponse.Message.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Register_InvalidInput_Returns400BadRequest_WithValidationErrors()
    {
        var command = new RegisterUserCommand
        {
            Username = "",
            Email = "invalid-email",
            Password = "short"
        };
        var response = await _client.PostAsJsonAsync("/api/auth/register", command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
        problemDetails.Errors.Should().ContainKeys("Username", "Email", "Password");

        var notRegisteredUser = await GetUserAsync(u => u.Username == command.Username && u.Email == command.Email);
        notRegisteredUser.Should().BeNull();
    }
}