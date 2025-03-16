using CSharpApp.Application.Commands.Products.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Products;

public static class CreateProductsEndpoint
{
    public const string Name = "CreateProduct";

    public static IEndpointRouteBuilder MapCreateProducts(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Products.Create, async ([FromBody] CreateProductCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var product = await mediator.Send(command, cancellationToken);
            if (product is null) return Results.BadRequest();
            return TypedResults.CreatedAtRoute(product, GetProductsByIdEndpoint.Name, new { productId = product!.Id });
        })
        .WithName(Name)
        .WithApiVersionSet(ApiVersioning.VersionSet!)
        .HasApiVersion(1.0);

        return app;
    }
}