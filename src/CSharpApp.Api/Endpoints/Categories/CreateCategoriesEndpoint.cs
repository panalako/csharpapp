using System.Security.Authentication;
using System.Text.Json;
using CSharpApp.Application.Commands.Categories.CreateCategories;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Dtos.AuthDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Endpoints.Categories;

public static class CreateCategoriesEndpoint
{
    public const string Name = "CreateCategories";

    public static IEndpointRouteBuilder MapCreateCategories(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Categories.Create, async ([FromBody] CreateCategoriesCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            try
            {
                var category = await mediator.Send(command, cancellationToken);
                return TypedResults.CreatedAtRoute(category, GetCategoriesByIdEndpoint.Name, new { categoryId = category!.Id });
            }
            catch (AuthenticationException ex)
            {
                return Results.BadRequest(JsonSerializer.Deserialize<AuthendiationFailureDto>(ex.Message));
            } 
            catch (HttpRequestException ex)
            {
                return Results.BadRequest(JsonSerializer.Deserialize<ForeignServerCreateErrorDto>(ex.Message));
            }
        })
        .WithName(Name)
        .WithApiVersionSet(ApiVersioning.VersionSet!)
        .HasApiVersion(1.0);

        return app;
    }
}