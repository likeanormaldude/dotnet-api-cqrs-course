using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger) : IMiddleware
{
    public const int LoggingTimeout = 4000;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        await next.Invoke(context);
        stopwatch.Stop();

        HttpRequest request = context.Request;
        double elapsedTimeInSeconds = stopwatch.ElapsedMilliseconds / 1000.0;

        if (stopwatch.ElapsedMilliseconds > LoggingTimeout)
        {
            logger.LogWarning(
                "Request [{Verb}] at {Path} took {time} s",
                request.Method,
                request.Path,
                elapsedTimeInSeconds.ToString("F3")
            );
        }
    }
}
