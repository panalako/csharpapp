namespace CSharpApp.Core.Dtos.CategoriesDtos;

public sealed class CategoriesResponse
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }
}