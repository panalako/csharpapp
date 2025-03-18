using System.Security.Authentication;
using System.Text.Json;
using CSharpApp.Application.Queries.Categories.GetAllCategories;
using CSharpApp.Core.Dtos.AuthDtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpApp.Api.Endpoints.Categories;

public static class GetAllCategoriesEndpoint
{
    public const string Name = "GetAllCategories";

    public static IEndpointRouteBuilder MapGetAllCategories(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Categories.GetAll, async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            try
            {
                var categories = await mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
                return Results.Ok(categories ?? []);
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