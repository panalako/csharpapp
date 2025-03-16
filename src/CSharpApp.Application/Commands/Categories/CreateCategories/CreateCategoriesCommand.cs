using System.Text.Json.Serialization;
using CSharpApp.Core.Dtos.CategoriesDtos;

namespace CSharpApp.Application.Commands.Categories.CreateCategories;

public class CreateCategoriesCommand : IRequest<CategoriesResponse?>
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }
}