namespace ClotheoAPI.Application.Auth.Context;

public record CurrentUser(int Id, string Username, string Email, bool IsAdmin)
{
}
