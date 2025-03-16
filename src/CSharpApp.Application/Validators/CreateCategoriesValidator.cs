using CSharpApp.Application.Commands.Categories.CreateCategories;
using FluentValidation;

namespace CSharpApp.Application.Validators;

public class CreateCategoriesValidator : AbstractValidator<CreateCategoriesCommand>
{
    public CreateCategoriesValidator()
    {
        RuleFor(value => value.Name).NotEmpty();
        RuleFor(value => value.Image).NotEmpty();  
    }
}