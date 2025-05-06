using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace ClotheoAPI.Application.Auth.Context.Tests;

public class UserContextTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly UserContext _userContext;

    public UserContextTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _userContext = new UserContext(_httpContextAccessorMock.Object);
    }

    [Fact]
    public void GetCurrentUser_AuthenticatedUser_ReturnsCurrentUser()
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Name, "testuser"),
            new(ClaimTypes.Email, "test@example.com"),
            new(ClaimTypes.Role, "User")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);

        _httpContextAccessorMock
            .Setup(a => a.HttpContext!.User)
            .Returns(user);

        var currentUser = _userContext.GetCurrentUser();

        currentUser.Should().NotBeNull();
        currentUser!.Id.Should().Be(1);
        currentUser.Username.Should().Be("testuser");
        currentUser.Email.Should().Be("test@example.com");
        currentUser.IsAdmin.Should().BeFalse();
    }

    [Fact]
    public void GetCurrentUser_NotAuthenticatedUser_ReturnsNull()
    {
        _httpContextAccessorMock
            .Setup(a => a.HttpContext!.User.Identity!.IsAuthenticated)
            .Returns(false);

        var currentUser = _userContext.GetCurrentUser();

        currentUser.Should().BeNull();
    }
}