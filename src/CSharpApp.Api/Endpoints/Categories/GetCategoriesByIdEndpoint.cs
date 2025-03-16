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
            var category = await mediator.Send(new GetCategoriesByIdQuery(categoryId), cancellationToken);
            
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