using CSharpApp.Application.Queries.Products.GetAllProducts;
using MediatR;

namespace CSharpApp.Api.Endpoints.Products;

public static class GetallProductsEndpoint
{
    public const string Name = "GetAllProducts";

    public static IEndpointRouteBuilder MapGetAllProducts(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Products.GetAll, async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var products = await mediator.Send(new GetAllProductsQuery(), cancellationToken);
            return products ?? [];
        })
        .WithName(Name)
        .WithApiVersionSet(ApiVersioning.VersionSet!)
        .HasApiVersion(1.0);

        return app;
    }
}