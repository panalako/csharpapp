using CSharpApp.Application.Queries.Products.GetProductsById;
using FluentValidation;

namespace CSharpApp.Application.Validators.Categories;

public class GetProductsByIdValidator : AbstractValidator<GetProductsByIdQuery>
{
    public GetProductsByIdValidator()
    {
        RuleFor(value => value.ProductId).GreaterThan(0).LessThanOrEqualTo(int.MaxValue).WithMessage("The product id must be greater that 0");
        
    }
}