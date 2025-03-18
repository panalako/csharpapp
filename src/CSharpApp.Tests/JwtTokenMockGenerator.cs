using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CSharpApp.Tests;

public class JwtTokenGenerator
{
    public static (string accessToken, string refreshToken) GenerateTokens(DateTime accessTokenExpire, DateTime refreshTokenExpire, string secretKey="Your_Secret_Key_That_Is_At_Least_32_Characters_Long")
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var accessTokenHandler = new JwtSecurityTokenHandler();
        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", "1"),
                new Claim("iat", iat.ToString(), ClaimValueTypes.Integer64),
                new Claim("exp", new DateTimeOffset(accessTokenExpire).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            }),
            Expires = accessTokenExpire,
            SigningCredentials = creds
        };
        var accessToken = accessTokenHandler.CreateToken(accessTokenDescriptor);
        string accessTokenString = accessTokenHandler.WriteToken(accessToken);

        var refreshTokenHandler = new JwtSecurityTokenHandler();
        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", "1"),
                new Claim("iat", iat.ToString(), ClaimValueTypes.Integer64),
                new Claim("exp", new DateTimeOffset(refreshTokenExpire).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            }),
            Expires = refreshTokenExpire,
            SigningCredentials = creds
        };
        var refreshToken = refreshTokenHandler.CreateToken(refreshTokenDescriptor);
        string refreshTokenString = refreshTokenHandler.WriteToken(refreshToken);

        return (accessTokenString, refreshTokenString);
    }
}
