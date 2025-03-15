namespace CSharpApp.Api.Endpoints.Products;

public static class GetallProductsEndpoint
{
    public const string Name = "GetProducts";

    public static IEndpointRouteBuilder MapGetAllProducts(this IEndpointRouteBuilder app)
    {
        // var versionedEndpointRouteBuilder = app.NewVersionedApi();
        app.MapGet(ApiEndpoints.Products.Create, async (IProductsService productsService) =>
        {
            var products = await productsService.GetProducts();
            return products;
        })
        .WithName(Name)
        .WithApiVersionSet(ApiVersioning.VersionSet!)
        .HasApiVersion(1.0);

        return app;
    }
}