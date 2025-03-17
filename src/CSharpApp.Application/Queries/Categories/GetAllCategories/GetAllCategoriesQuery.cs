using CSharpApp.Core.Dtos.CategoriesDtos;

namespace CSharpApp.Application.Queries.Categories.GetAllCategories;

public record GetAllCategoriesQuery() : IRequest<IReadOnlyCollection<CategoriesResponse>?>;
