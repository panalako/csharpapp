using CSharpApp.Application.Auth;

namespace CSharpApp.Infrastructure.Auth;

public static class AuthExtentions
{
    public static IServiceCollection AddAuthTokenProvider(this IServiceCollection services)
    {
       services.AddSingleton<AuthToken>(); 
        return services;
    }
}