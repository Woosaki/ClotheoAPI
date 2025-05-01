namespace ClotheoAPI.Domain.Exceptions;

/// <summary>
/// Exception thrown when the client is not authenticated.
/// </summary>
/// <param name="message">A message describing the authorization failure (optional).</param>
public class UnauthorizedException : Exception
{
    public UnauthorizedException()
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }
}
