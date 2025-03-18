
using System.Text;

namespace CSharpApp.Application.HttpClients;

public class AuthHttpClient(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings) : IAuthHttpClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;

    public async Task<HttpResponseMessage> RequestToken()
    {
        var path = _restApiSettings.Auth!;
        path = path.StartsWith('/') ? path.TrimStart('/') : path;

        var json = JsonSerializer.Serialize(new {
            email = _restApiSettings.Username,
            password = _restApiSettings.Password
        });
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        return await _httpClient.PostAsync(path, data);
    }

        public async Task<HttpResponseMessage> RefreshToken(string refreshToken)
    {
        var path = _restApiSettings.Auth!;
        path = path.StartsWith('/') ? path.TrimStart('/') : path;

        var json = JsonSerializer.Serialize(new {
            refreshToken
        });
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        return await _httpClient.PostAsync(path, data);
    }
}