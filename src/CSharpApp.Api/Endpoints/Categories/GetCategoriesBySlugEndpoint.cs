using System.Security.Authentication;
using System.Text.Json;
using CSharpApp.Application.Queries.Categories.GetCategoriesBySlug;
using CSharpApp.Core.Dtos.AuthDtos;
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
            try
            {
                var category = await mediator.Send(new GetCategoriesBySlugQuery(categorySlug), cancellationToken);

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