using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using CSharpApp.Core.Dtos.AuthDtos;

namespace CSharpApp.Application.Auth;


public class AuthService : IAuthService
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private IAuthHttpClient _httpClient { get; set; }
    private readonly ILogger<AuthService> _logger;
    private IAuthToken _authToken { get; set; }

    public AuthService(IAuthHttpClient httpClient, ILogger<AuthService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _authToken = new AuthToken();
    }

    public async Task<string?> GetAccessToken()
    {
        if (!string.IsNullOrEmpty(_authToken.AccessToken))
        {
            return _authToken.AccessToken;
        }

        if (string.IsNullOrEmpty(_authToken.AccessToken) || string.IsNullOrEmpty(_authToken.RefreshToken))
        {
            await RetriveTokenAsync();
        }

        if (_authToken.HasAccessTokenExpired())
        {
            if (_authToken.HasRefreshTokenExpired())
            {
                await RetriveTokenAsync();

            }
            else
            {
                await RefreshAccessToken();
            }
        }

        return _authToken.AccessToken ?? null;

    }

    public async Task RetriveTokenAsync()
    {
        await _lock.WaitAsync();

        try
        {
            var response = await _httpClient.RequestToken();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);

                if (tokenResponse != null)
                {
                    var accessTokenExpiration = GetTokenExpirationDate(tokenResponse.AccessToken);
                    var refreshTokenExpiration = GetTokenExpirationDate(tokenResponse.RefreshToken);

                    _authToken.AccessToken = tokenResponse.AccessToken;
                    _authToken.RefreshToken = tokenResponse.RefreshToken;
                    _authToken.AccessTokenExpiration = accessTokenExpiration;
                    _authToken.RefreshTokenExpiration = refreshTokenExpiration;
                }
            }
            else
            {
                throw new AuthenticationException($"{response.Content.ReadAsStringAsync().Result}");
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task RefreshAccessToken()
    {
        var response = await _httpClient.RefreshToken(_authToken.RefreshToken!);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);

            if (tokenResponse != null)
            {
                var accessTokenExpiration = GetTokenExpirationDate(tokenResponse.AccessToken);
                var refreshTokenExpiration = GetTokenExpirationDate(tokenResponse.RefreshToken);

                _authToken.AccessToken = tokenResponse.AccessToken;
                _authToken.RefreshToken = tokenResponse.RefreshToken;
                _authToken.AccessTokenExpiration = accessTokenExpiration;
                _authToken.RefreshTokenExpiration = refreshTokenExpiration;
            }
        }
        else
        {
            _logger.LogError("Falied to refresh the token, error {refreshTokenError}", response.StatusCode);
        }
    }

    private static DateTime GetTokenExpirationDate(string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var expiryTimeText = jwt.Claims.Single(claim => claim.Type == "exp").Value;
        var expiryDateTime = UnixTimeStampToDateTime(int.Parse(expiryTimeText));
        return expiryDateTime;
    }

    private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }

}