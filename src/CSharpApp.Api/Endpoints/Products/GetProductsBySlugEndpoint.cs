using System.Security.Authentication;
using System.Text.Json;
using CSharpApp.Application.Queries.Products.GetProductsBySlug;
using CSharpApp.Core.Dtos.AuthDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Products;

public static class GetProductsBySlugEndpoint
{
    public const string Name = "GetProductsBySlug";

    public static IEndpointRouteBuilder MapGetProductsBySlug(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Products.GetBySlug, async ([FromRoute] string productSlug, IMediator mediator, CancellationToken cancellationToken) =>
        {
            try
            {
                var product = await mediator.Send(new GetProductsBySlugQuery(productSlug), cancellationToken);

                if (product is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(product);
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