using CSharpApp.Application.Auth;

namespace CSharpApp.Api.Middlewares.AuthMiddlewares;

public class TokenRefreshMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenRefreshMiddleware> _logger;

    public TokenRefreshMiddleware(RequestDelegate next, ILogger<TokenRefreshMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        await _next(context);
    }
}