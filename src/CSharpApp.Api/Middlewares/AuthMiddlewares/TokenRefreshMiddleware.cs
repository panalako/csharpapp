using CSharpApp.Application.Auth;

namespace CSharpApp.Api.Middlewares.AuthMiddlewares;

public class TokenRefreshMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AuthToken _authToken;
    private readonly IAuthHttpClient _httpClient;
    private readonly ILogger<TokenRefreshMiddleware> _logger;

    public TokenRefreshMiddleware(RequestDelegate next, AuthToken token, IAuthHttpClient httpClient, ILogger<TokenRefreshMiddleware> logger)
    {
        _next = next;
        _authToken = token;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_authToken.HasAccessTokenExpired())
        {
            if (_authToken.HasRefreshTokenExpired())
            {
                await _authToken.InitializeTokenAsync();
            }
            else
            {
                await RefreshAccessToken();
            }
        }

        await _next(context);
    }

    private async Task RefreshAccessToken()
    {
        var response = await _httpClient.RefreshToken(_authToken.RefreshToken!);

        if (response.IsSuccessStatusCode)
        {
            await _authToken.SetTokenAsync(response);
        }
        else
        {
            _logger.LogError("Falied to refresh the token, error {refreshTokenError}", response.StatusCode);
        }
    }
}