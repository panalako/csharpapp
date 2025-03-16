namespace CSharpApp.Core.Interfaces;

public interface ICoreHttpClient
{
    Task<HttpResponseMessage?> GetHttpResponseMessageAsync(string path, CancellationToken cancellationToken = default);
}