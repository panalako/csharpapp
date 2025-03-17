namespace CSharpApp.Application.Queries.Products.GetAllProducts;

public record GetAllProductsQuery() : IRequest<IReadOnlyCollection<Product>?>;
