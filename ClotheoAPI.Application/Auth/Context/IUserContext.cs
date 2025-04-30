namespace ClotheoAPI.Application.Auth.Context;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}