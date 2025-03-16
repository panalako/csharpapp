using CSharpApp.Application.Queries.Categories.GetCategoriesById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Categories;

public static class GetCategoriesByIdEndpoint
{
    public const string Name = "GetCategoriesById";

    public static IEndpointRouteBuilder MapGetCategoriesById(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Categories.GetById, async ([FromRoute] int categoryId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var product = await mediator.Send(new GetCategoriesByIdQuery(categoryId), cancellationToken);
            
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