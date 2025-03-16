namespace CSharpApp.Application.Queries.Products.GetProductsBySlug;

public record GetProductsBySlugQuery(string ProductSlug) : IRequest<Product?>;