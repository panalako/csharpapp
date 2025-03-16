using CSharpApp.Core.Dtos.CategoriesDtos;

namespace CSharpApp.Application.Queries.Categories.GetCategoriesById;

public record GetCategoriesByIdQuery(int CategoryId) : IRequest<CategoriesResponse?>;