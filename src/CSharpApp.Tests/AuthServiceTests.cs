using System.Net;
using System.Text;
using CSharpApp.Application.Auth;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace CSharpApp.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task AuthService_ShouldReturnToken_WhenAuthIsSuccessful()
    {
        var mockHttpClient = new Mock<IAuthHttpClient>();
        var mockLogger = new Mock<ILogger<AuthService>>();
        DateTime accessTokenExpire = DateTime.UtcNow.AddMinutes(15);
        DateTime refreshTokenExpire = DateTime.UtcNow.AddDays(7);
        var (accessToken, refreshToken) = JwtTokenGenerator.GenerateTokens(accessTokenExpire, refreshTokenExpire);

        // Fake JSON response
        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new
            {
                access_token = accessToken,
                refresh_token = refreshToken
            }), Encoding.UTF8, "application/json")
        };

        mockHttpClient
            .Setup(client => client.RequestToken())
            .ReturnsAsync(fakeResponse);    

        var authService = new AuthService(mockHttpClient.Object, mockLogger.Object);

        var resultAccessToken = await authService.GetAccessToken();

        Assert.NotNull(resultAccessToken);
        Assert.Equal(accessToken, resultAccessToken);
    }

    [Fact]
    public async Task AuthService_ShouldRefreshAndReturnToken_WhenAuthIsSuccessful()
    {
        var mockHttpClient = new Mock<IAuthHttpClient>();
        var mockLogger = new Mock<ILogger<AuthService>>();
        DateTime accessTokenExpire = DateTime.UtcNow.AddSeconds(1);
        DateTime refreshTokenExpire = DateTime.UtcNow.AddDays(7);
        var (accessToken, refreshToken) = JwtTokenGenerator.GenerateTokens(accessTokenExpire, refreshTokenExpire);

        // Fake JSON response
        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new
            {
                access_token = accessToken,
                refresh_token = refreshToken
            }), Encoding.UTF8, "application/json")
        };

        mockHttpClient
            .Setup(client => client.RequestToken())
            .ReturnsAsync(fakeResponse);


        var (accessRefreshedToken, refreshRefreshedToken) = JwtTokenGenerator.GenerateTokens(accessTokenExpire.AddHours(15), refreshTokenExpire);
        var fakeRefreshResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new
            {
                access_token = accessRefreshedToken,
                refresh_token = refreshRefreshedToken
            }), Encoding.UTF8, "application/json")
        };


        mockHttpClient
            .Setup(client => client.RefreshToken(refreshToken))
            .ReturnsAsync(fakeRefreshResponse);    

        await Task.Delay(2000);
        var authService = new AuthService(mockHttpClient.Object, mockLogger.Object);

        var resultAccessToken = await authService.GetAccessToken();

        Assert.NotNull(resultAccessToken);
        Assert.Equal(accessRefreshedToken, resultAccessToken);
    }
}