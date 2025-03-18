using System.Net.Http.Headers;
using CSharpApp.Application.Auth;

namespace CSharpApp.Application.HttpClients;

public class CoreHttpClient(HttpClient httpClient, IAuthService authService ) : ICoreHttpClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IAuthService _authService = authService;

    public async Task<HttpResponseMessage?> GetHttpResponseMessageAsync(string path, CancellationToken cancellationToken = default)
    {
        string? token = await _authService.GetAccessToken();
        if (string.IsNullOrEmpty(token)) return null;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        path = path.StartsWith('/') ? path.TrimStart('/') : path;
        return await _httpClient.GetAsync(path, cancellationToken);
    }

    public async Task<HttpResponseMessage?> GetHttpResponseMessageAsync(string path, StringContent data, CancellationToken cancellationToken = default)
    {
        string? token = await _authService.GetAccessToken();
        if (string.IsNullOrEmpty(token)) return null;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        path = path.StartsWith('/') ? path.TrimStart('/') : path;
        return await _httpClient.PostAsync(path, data, cancellationToken);
    }
}