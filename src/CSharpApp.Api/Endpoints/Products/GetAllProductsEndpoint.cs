using System.Security.Authentication;
using System.Text.Json;
using CSharpApp.Application.Queries.Products.GetAllProducts;
using CSharpApp.Core.Dtos.AuthDtos;
using MediatR;

namespace CSharpApp.Api.Endpoints.Products;

public static class GetallProductsEndpoint
{
    public const string Name = "GetAllProducts";

    public static IEndpointRouteBuilder MapGetAllProducts(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Products.GetAll, async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            try
            {
                var products = await mediator.Send(new GetAllProductsQuery(), cancellationToken);
                return Results.Ok(products ?? []);
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
