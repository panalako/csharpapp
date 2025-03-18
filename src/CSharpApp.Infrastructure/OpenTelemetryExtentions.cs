using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CSharpApp.Infrastructure;

public static class OpenTelemetryExtentions
{
    private const string AppName = "CSharpApp";

    public static ILoggingBuilder IncludeOpenTelemetryLogs(this ILoggingBuilder loggingBuilder)
    {
        var openTelemetrySettings = loggingBuilder.Services.BuildServiceProvider().GetRequiredService<IOptions<OpenTelemetrySettings>>().Value;
        loggingBuilder.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(AppName))
            .AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(openTelemetrySettings.AspireUrl!);
            });
        });

        return loggingBuilder;
    }

    public static IServiceCollection IncludeOpenTelemetry(this IServiceCollection services)
    {
        var openTelemetrySettings = services.BuildServiceProvider().GetRequiredService<IOptions<OpenTelemetrySettings>>().Value;
        services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(AppName))
        .WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation()
            .AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(openTelemetrySettings.AspireUrl!);
            });
        })
        .WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation()
            .AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(openTelemetrySettings.AspireUrl!);
            });
        });

        return services;
    }
}
