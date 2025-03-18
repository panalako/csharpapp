using System.Security.Authentication;
using System.Text.Json;
using CSharpApp.Application.Queries.Products.GetProductsById;
using CSharpApp.Core.Dtos.AuthDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Products;

public static class GetProductsByIdEndpoint
{
    public const string Name = "GetProductsById";

    public static IEndpointRouteBuilder MapGetProductsById(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Products.GetById, async ([FromRoute] int productId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            try
            {
                var product = await mediator.Send(new GetProductsByIdQuery(productId), cancellationToken);

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