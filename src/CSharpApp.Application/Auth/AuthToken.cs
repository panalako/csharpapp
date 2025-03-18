namespace CSharpApp.Application.Auth;

public class AuthToken : IAuthToken
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? AccessTokenExpiration { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
    public bool HasAccessTokenExpired() => DateTime.UtcNow >= AccessTokenExpiration;
    public bool HasRefreshTokenExpired() => DateTime.UtcNow >= RefreshTokenExpiration;
}