using CSharpApp.Application.Validators.Categories;
using FluentValidation;

namespace CSharpApp.Infrastructure.ValidationExtentions;

public static class ValidationExtentions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<GetProductsByIdValidator>(ServiceLifetime.Singleton);
        return services;
    }
}