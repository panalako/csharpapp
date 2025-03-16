using CSharpApp.Application.Commands.Categories.CreateCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Categories;

public static class CreateCategoriesEndpoint
{
    public const string Name = "CreateCategories";

    public static IEndpointRouteBuilder MapCreateCategories(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Categories.Create, async ([FromBody] CreateCategoriesCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var category = await mediator.Send(command, cancellationToken);
            if (category is null) return Results.BadRequest();
            return TypedResults.CreatedAtRoute(category, GetCategoriesByIdEndpoint.Name, new { categoryId = category!.Id });
        })
        .WithName(Name)
        .WithApiVersionSet(ApiVersioning.VersionSet!)
        .HasApiVersion(1.0);

        return app;
    }
}