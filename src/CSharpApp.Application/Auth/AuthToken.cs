using System.IdentityModel.Tokens.Jwt;
using CSharpApp.Core.Dtos.AuthDtos;

namespace CSharpApp.Application.Auth;

public class AuthToken
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private IAuthHttpClient _httpClient { get; set; }
    private readonly ILogger<AuthToken> _logger;
    public string? AccessToken { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? AccessTokenExpiration { get; private set; }
    public DateTime? RefreshTokenExpiration { get; private set; }
    public bool HasAccessTokenExpired() => DateTime.UtcNow >= AccessTokenExpiration;
    public bool HasRefreshTokenExpired() => DateTime.UtcNow >= RefreshTokenExpiration;

    public AuthToken(IAuthHttpClient httpClient, ILogger<AuthToken> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        InitializeTokenAsync().Wait();
    }

    public async Task InitializeTokenAsync()
    {
        var response = await _httpClient.RequestToken();

        if (response.IsSuccessStatusCode)
        {
            await SetTokenAsync(response);
        }
        else
        {
            _logger.LogError("Failed to retrive the token {tokenRetriveError}", response.StatusCode);
            await UpdateTokensAsync(string.Empty, string.Empty, null, null);
        }
    }

    public async Task UpdateTokensAsync(string accessToken, string refreshToken, DateTime? accessTokenExpiration, DateTime? refreshTokenExpiration)
    {
        await _lock.WaitAsync();
        try
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            AccessTokenExpiration = accessTokenExpiration;
            RefreshTokenExpiration = refreshTokenExpiration;
        }
        catch (Exception ex)
        {
            _logger.LogError("Falied to update the token with error {tokenUpdateError}", ex.Message);
            AccessToken = string.Empty;
            RefreshToken = string.Empty;
            AccessTokenExpiration = null;
            RefreshTokenExpiration = null;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task SetTokenAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);

        if (tokenResponse != null)
        {
            var accessTokenExpiration = GetTokenExpirationDate(tokenResponse.AccessToken);
            var refreshTokenExpiration = GetTokenExpirationDate(tokenResponse.RefreshToken);
            await UpdateTokensAsync(tokenResponse.AccessToken, tokenResponse.RefreshToken, accessTokenExpiration, refreshTokenExpiration);
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