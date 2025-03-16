using System.Diagnostics;

namespace CSharpApp.Api.Middlewares.PerformanceMiddlewares;

public class PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<PerformanceMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        await _next(context);
        
        stopwatch.Stop();

        var elapsedTime = stopwatch.ElapsedMilliseconds;
        _logger.LogInformation("Request: {context.Request.Method} with Path: {context.Request.Path} took {elapsedTime} ms.", context.Request.Method, context.Request.Path, elapsedTime);
    }
}