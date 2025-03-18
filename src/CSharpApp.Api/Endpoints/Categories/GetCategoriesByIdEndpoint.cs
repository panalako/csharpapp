using System.Security.Authentication;
using System.Text.Json;
using CSharpApp.Application.Queries.Categories.GetCategoriesById;
using CSharpApp.Core.Dtos.AuthDtos;
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
            try
            {
                var category = await mediator.Send(new GetCategoriesByIdQuery(categoryId), cancellationToken);

                if (category is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(category);
            }
            catch (AuthenticationException ex)
            {
                return Results.BadRequest(JsonSerializer.Deserialize<AuthendiationFailureDto>(ex.Message));
            }

        })
        .WithName(Name)
        .WithApiVersionSet(ApiVersioning.VersionSet!)
        .HasApiVersion(1.0);

        return app;
    }
}