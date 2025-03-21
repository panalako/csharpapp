using CSharpApp.Api.Endpoints.Categories;
using CSharpApp.Api.Endpoints.Products;

namespace CSharpApp.Api.Endpoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapProductEndpoints();
        app.MapCategoriesEndpoints();
        return app;
    }
}