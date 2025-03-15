using CSharpApp.Core.HttpClients;
using Microsoft.Extensions.Options;

namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        var apiSettings = services.BuildServiceProvider().GetRequiredService<IOptions<RestApiSettings>>();
        var httpClientSettings = services.BuildServiceProvider().GetRequiredService<IOptions<HttpClientSettings>>();
        
        services.AddHttpClient<ICoreHttpClient, CoreHttpClient>(client => {
            client.BaseAddress = new Uri(apiSettings.Value.BaseUrl!);
        });

        return services;
    }
}