namespace CSharpApp.Application.Queries.Products.GetProductsById;

public record GetProductsByIdQuery(int ProductId) : IRequest<Product?>;