﻿namespace ClotheoAPI.Domain.Exceptions;

/// <summary>
/// Exception thrown when the client is authenticated but does not have permission to access the resource.
/// </summary>
/// <param name="message">A message describing the permission failure (optional).</param>
public class ForbiddenException : Exception
{
    public ForbiddenException()
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }
}
