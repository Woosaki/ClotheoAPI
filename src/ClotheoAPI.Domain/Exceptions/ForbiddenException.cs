namespace ClotheoAPI.Domain.Exceptions;

/// <summary>
/// Exception thrown when the client is authenticated but does not have permission to access the resource.
/// </summary>
/// <param name="message">A message describing the permission failure (optional).</param>
public class ForbiddenException(string message = "You do not have permission to perform this action.")
    : Exception(message)
{
}
