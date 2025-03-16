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
            var product = await mediator.Send(new GetCategoriesBySlugQuery(categorySlug), cancellationToken);
            
            if (product is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(product);

        })
        .WithName(Name)
        .WithApiVersionSet(ApiVersioning.VersionSet!)
        .HasApiVersion(1.0);

        return app;
    }
}