namespace ClotheoAPI.Domain.Exceptions;

/// <summary>
/// Exception thrown when the client is not authenticated.
/// </summary>
/// <param name="message">A message describing the authorization failure (optional).</param>
public class UnauthorizedException(string message = "You must be authenticated to access this resource.")
    : Exception(message)
{
}
