namespace ClotheoAPI.Domain.Exceptions;

/// <summary>
/// Exception thrown when the request is invalid or malformed.
/// </summary>
/// <param name="message">A message describing the invalid request.</param>
public class BadRequestException(string message)
    : Exception(message)
{
}
