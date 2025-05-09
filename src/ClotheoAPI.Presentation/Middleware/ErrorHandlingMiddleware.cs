
using ClotheoAPI.Domain.Common;
using ClotheoAPI.Domain.Exceptions;
using System.Net;

namespace ClotheoAPI.Presentation.Middleware;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = ex.Message;

        switch (ex)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;
            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                break;
            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                break;
            case ForbiddenException:
                statusCode = HttpStatusCode.Forbidden;
                break;
            default:
                message = "An unexpected error occurred.";
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        var errorResponse = new ErrorResponse(context.Response.StatusCode, message);

        logger.LogError(ex, message);
        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}
