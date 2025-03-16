namespace CSharpApp.Core.Interfaces;

public interface ICoreHttpClient
{
    Task<HttpResponseMessage?> GetHttpResponseMessageAsync(string path, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage?> GetHttpResponseMessageAsync(string path, StringContent data, CancellationToken cancellationToken = default);
}