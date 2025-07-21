using FluentValidation;
using Newtonsoft.Json;
using Restaurants.Application.Extensions;

namespace Restaurants.API.Middlewares;

public class ValidationExceptionMiddleware(ILogger<ValidationExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            HttpRequest request = context.Request;
            logger.LogWarning("Request [{Verb}] at {Path} threw ValidationErrors", request.Method, request.Path);

            string serializedErrors = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);
            logger.LogError("{@Errors}", serializedErrors);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(ex.Errors.ToGroupedValidationErrors().MapOnlyErrorMessages());
        }
    }
}
