using CSharpApp.Application.Queries.Categories.GetAllCategories;
using MediatR;

namespace CSharpApp.Api.Endpoints.Categories;

public static class GetAllCategoriesEndpoint
{
    public const string Name = "GetAllCategories";

    public static IEndpointRouteBuilder MapGetAllCategories(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Categories.GetAll, async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var products = await mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
            return products ?? [];
        })
        .WithName(Name)
        .WithApiVersionSet(ApiVersioning.VersionSet!)
        .HasApiVersion(1.0);

        return app;
    }
}