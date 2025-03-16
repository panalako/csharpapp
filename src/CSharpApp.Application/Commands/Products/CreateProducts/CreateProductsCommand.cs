using System.Text.Json.Serialization;

namespace CSharpApp.Application.Commands.Products.CreateProducts;

public class CreateProductsCommand : IRequest<Product?>
{
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("price")]
    public int? Price { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("images")]
    public IEnumerable<string> Images { get; set; } = [];

    [JsonPropertyName("categoryId")]
    public int categoryId { get; init; }
}