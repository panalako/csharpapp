namespace CSharpApp.Application.Queries.Categories.GetAllCategories;

public record GetAllCategoriesQuery() : IRequest<IReadOnlyCollection<Category>>;
