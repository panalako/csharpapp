using CSharpApp.Core.Dtos.CategoriesDtos;

namespace CSharpApp.Application.Queries.Categories.GetCategoriesBySlug;

public record GetCategoriesBySlugQuery(string CategorySlug) : IRequest<CategoriesResponse?>;