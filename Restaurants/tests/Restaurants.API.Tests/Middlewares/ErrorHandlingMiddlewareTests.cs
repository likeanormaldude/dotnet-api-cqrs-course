using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Middlewares.Tests;

public class ErrorHandlingMiddlewareTests
{
    [Fact()]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        #region Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();
        #endregion

        #region Act
        await middleware.InvokeAsync(context, nextDelegateMock.Object);
        #endregion

        #region Assert
        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
        #endregion
    }

    [Fact()]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
    {
        #region Arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var exception = new NotFoundException(nameof(Restaurant), "1");
        #endregion

        #region Act
        await middleware.InvokeAsync(
            context,
            _ =>
            {
                throw exception;
            }
        );
        #endregion

        #region Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        #endregion
    }

    [Fact()]
    public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode403()
    {
        #region Arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var exception = new ForbidException();
        #endregion

        #region Act
        await middleware.InvokeAsync(
            context,
            _ =>
            {
                throw exception;
            }
        );
        #endregion

        #region Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        #endregion
    }

    [Fact()]
    public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
    {
        #region Arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var exception = new Exception();
        #endregion

        #region Act
        await middleware.InvokeAsync(
            context,
            _ =>
            {
                throw exception;
            }
        );
        #endregion

        #region Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        #endregion
    }
}
