using ClotheoAPI.Domain.Entities;
using ClotheoAPI.Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Xunit;

namespace ClotheoAPI.Presentation.Middleware.Tests;

public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
    private readonly Mock<RequestDelegate> _nextMock;
    private readonly ErrorHandlingMiddleware _middleware;
    private readonly DefaultHttpContext _httpContext;

    public ErrorHandlingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _nextMock = new Mock<RequestDelegate>();
        _middleware = new ErrorHandlingMiddleware(_loggerMock.Object);
        _httpContext = new DefaultHttpContext();
    }

    [Fact]
    public async Task InvokeAsync_NoException_CallsNextMiddleware()
    {
        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        _nextMock.Verify(next => next.Invoke(_httpContext), Times.Once);
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task InvokeAsync_NotFoundException_SetsStatusCode404()
    {
        var exception = new NotFoundException(nameof(User), 1);
        _nextMock
            .Setup(next => next.Invoke(It.IsAny<HttpContext>()))
            .ThrowsAsync(exception);

        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        _httpContext.Response.ContentType.Should().StartWith("application/json");
        exception.Message.Should().Be("User with ID '1' was not found.");
    }

    [Fact]
    public async Task InvokeAsync_BadRequestException_SetsStatusCode400()
    {
        var exception = new BadRequestException("Test went wrong.");
        _nextMock
            .Setup(next => next.Invoke(It.IsAny<HttpContext>()))
            .ThrowsAsync(exception);

        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        _httpContext.Response.ContentType.Should().StartWith("application/json");
        exception.Message.Should().Be("Test went wrong.");
    }

    [Fact]
    public async Task InvokeAsync_UnauthorizedException_SetsStatusCode401()
    {
        var exception = new UnauthorizedException();
        _nextMock
            .Setup(next => next.Invoke(It.IsAny<HttpContext>()))
            .ThrowsAsync(exception);

        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        _httpContext.Response.ContentType.Should().StartWith("application/json");
        exception.Message.Should().Be("You must be authenticated to access this resource.");
    }

    [Fact]
    public async Task InvokeAsync_ForbiddenException_SetsStatusCode403()
    {
        var exception = new ForbiddenException();
        _nextMock
            .Setup(next => next.Invoke(It.IsAny<HttpContext>()))
            .ThrowsAsync(exception);

        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        _httpContext.Response.ContentType.Should().StartWith("application/json");
        exception.Message.Should().Be("You do not have permission to perform this action.");
    }
}