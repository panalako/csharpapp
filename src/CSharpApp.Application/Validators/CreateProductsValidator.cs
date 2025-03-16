using CSharpApp.Application.Commands.Products.CreateProduct;
using FluentValidation;

namespace CSharpApp.Application.Validators;

public class CreateProductsValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductsValidator()
    {
        RuleFor(value => value.Title).NotEmpty();
        RuleFor(value => value.Price).GreaterThanOrEqualTo(0).LessThanOrEqualTo(int.MaxValue).NotNull();
        RuleFor(value => value.Description).NotEmpty();
        RuleFor(value => value.categoryId).NotEmpty();
        RuleFor(value => value.Images).NotNull();  
    }
}