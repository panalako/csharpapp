namespace CSharpApp.Api.Endpoints.Categories;

public static class CategoriesEndpointExetensions
{
    public static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetAllCategories();
        app.MapGetCategoriesById();
        app.MapGetCategoriesBySlug();
        app.MapCreateCategories();
        return app;
    }
}