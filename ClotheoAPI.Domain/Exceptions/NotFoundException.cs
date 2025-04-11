namespace ClotheoAPI.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested resource was not found.
/// </summary>
/// <param name="resourceName">The name of the not found resource.</param>
/// <param name="id">The identifier of the not found resource.</param>
public class NotFoundException(string resourceName, object id)
    : Exception($"{resourceName} with ID: {id} was not found.")
{
}
