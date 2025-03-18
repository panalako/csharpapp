namespace CSharpApp.Core.Interfaces;

public interface IAuthToken
{
    string? AccessToken { get; set; }
    string? RefreshToken { get; set; }
    DateTime? AccessTokenExpiration { get; set; }
    DateTime? RefreshTokenExpiration { get; set; }
    bool HasAccessTokenExpired() => DateTime.UtcNow >= AccessTokenExpiration;
    bool HasRefreshTokenExpired() => DateTime.UtcNow >= RefreshTokenExpiration;
}