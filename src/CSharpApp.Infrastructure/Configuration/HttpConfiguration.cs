using CSharpApp.Core.HttpClients;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Timeout;

namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        var restApiSettings = services.BuildServiceProvider().GetRequiredService<IOptions<RestApiSettings>>().Value;
        var httpClientSettings = services.BuildServiceProvider().GetRequiredService<IOptions<HttpClientSettings>>().Value;

        services.AddHttpClient<ICoreHttpClient, CoreHttpClient>(client =>
        {
            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(httpClientSettings.LifeTime))
        .AddTransientHttpErrorPolicy(policy => policy.Or<TimeoutRejectedException>().WaitAndRetryAsync(
            httpClientSettings.RetryCount,
            retryAttempt => TimeSpan.FromMilliseconds(httpClientSettings.SleepDuration / 2)
        ))
        .AddTransientHttpErrorPolicy(policy => policy.Or<TimeoutRejectedException>().CircuitBreakerAsync(
            httpClientSettings.RetryCount,
            TimeSpan.FromSeconds(httpClientSettings.SleepDuration)
        ))
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

        return services;
    }
}