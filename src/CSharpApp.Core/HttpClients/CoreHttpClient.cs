using CSharpApp.Core.Interfaces;

namespace CSharpApp.Core.HttpClients;

public class CoreHttpClient(HttpClient httpClient) : ICoreHttpClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<HttpResponseMessage?> GetHttpResponseMessageAsync(string path, CancellationToken cancellationToken = default)
    {
        path = path.StartsWith('/') ? path.TrimStart('/') : path;
        return await _httpClient.GetAsync(path, cancellationToken);
    }
}