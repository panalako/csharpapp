using System.Text.Json;
using CSharpApp.Application.Commands.Products.CreateProducts;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Products;

public static class CreateProductsEndpoint
{
    public const string Name = "CreateProduct";

    public static IEndpointRouteBuilder MapCreateProducts(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Products.Create, async ([FromBody] CreateProductsCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            try
            {
                var product = await mediator.Send(command, cancellationToken);
                return TypedResults.CreatedAtRoute(product, GetProductsByIdEndpoint.Name, new { productId = product!.Id });
            }
            catch (HttpRequestException ex)
            {
                return Results.BadRequest(JsonSerializer.Deserialize<ForeignServerCreateErrorDto>(ex.Message));
            }

        })
        .WithName(Name)
        .WithApiVersionSet(ApiVersioning.VersionSet!)
        .HasApiVersion(1.0);

        return app;
    }
}