using System.Net.Http.Headers;
using CSharpApp.Application.Auth;

namespace CSharpApp.Application.HttpClients;

public class CoreHttpClient(HttpClient httpClient, AuthToken authToken) : ICoreHttpClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly AuthToken _authToken = authToken;

    public async Task<HttpResponseMessage?> GetHttpResponseMessageAsync(string path, CancellationToken cancellationToken = default)
    {
        string? token = _authToken.AccessToken;
        if (string.IsNullOrEmpty(token)) return null;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        path = path.StartsWith('/') ? path.TrimStart('/') : path;
        return await _httpClient.GetAsync(path, cancellationToken);
    }

    public async Task<HttpResponseMessage?> GetHttpResponseMessageAsync(string path, StringContent data, CancellationToken cancellationToken = default)
    {
        string? token = _authToken.AccessToken;
        if (string.IsNullOrEmpty(token)) return null;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        path = path.StartsWith('/') ? path.TrimStart('/') : path;
        return await _httpClient.PostAsync(path, data, cancellationToken);
    }
}