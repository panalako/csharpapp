using CSharpApp.Application.Queries.Categories.GetCategoriesById;
using FluentValidation;

namespace CSharpApp.Application.Validators.Categories;

public class GetCategoriesByIdValidator : AbstractValidator<GetCategoriesByIdQuery>
{
    public GetCategoriesByIdValidator()
    {
        RuleFor(value => value.CategoryId).GreaterThan(0).LessThanOrEqualTo(int.MaxValue).WithMessage("The product id must be greater that 0");
        
    }
}