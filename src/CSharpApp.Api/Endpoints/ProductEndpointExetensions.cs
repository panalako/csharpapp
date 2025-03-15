using CSharpApp.Api.Endpoints.Products;

namespace CSharpApp.Api.Endpoints;

public static class ProductEndpointExetensions
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetAllProducts();
        return app;
    }
}