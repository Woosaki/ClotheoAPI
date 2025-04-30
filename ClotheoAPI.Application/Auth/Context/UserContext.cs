using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ClotheoAPI.Application.Auth.Context;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var id = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var username = user.FindFirstValue(ClaimTypes.Name)!;
        var email = user.FindFirstValue(ClaimTypes.Email)!;
        var isAdmin = user.IsInRole("Admin");

        return new CurrentUser(id, username, email, isAdmin);
    }
}
