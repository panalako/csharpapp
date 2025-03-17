namespace CSharpApp.Core.Interfaces;

public interface IAuthHttpClient
{
    Task<HttpResponseMessage> RequestToken();
    Task<HttpResponseMessage> RefreshToken(string refreshToken);
}