namespace ClotheoAPI.Domain.Common;

public class ErrorResponse(int statusCode, string message)
{
    public int StatusCode { get; } = statusCode;
    public string Message { get; } = message;
}
