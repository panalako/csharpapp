using CSharpApp.Application.Queries.Categories.GetCategoriesBySlug;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Categories;

public static class GetCategoriesBySlugEndpoint
{
    public const string Name = "GetCategoriesBySlug";

    public static IEndpointRouteBuilder MapGetCategoriesBySlug(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Categories.GetBySlug, async ([FromRoute] string categorySlug, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var category = await mediator.Send(new GetCategoriesBySlugQuery(categorySlug), cancellationToken);
            
            if (category is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(category);

        })
        .WithName(Name)
        .WithApiVersionSet(ApiVersioning.VersionSet!)
        .HasApiVersion(1.0);

        return app;
    }
}