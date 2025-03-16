namespace CSharpApp.Api.Endpoints.Products;

public static class ProductEndpointExetensions
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetAllProducts();
        app.MapGetProductsById();
        return app;
    }
}