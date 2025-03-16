using CSharpApp.Core.Dtos.ValidationDtos.Responses;
using FluentValidation;
using MediatR;

namespace CSharpApp.Api.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Get all validators for the request
        var context = new ValidationContext<TRequest>(request);
        var validationFailures = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        // If there are validation errors, throw an exception
        if (validationFailures.Any())
        {
            throw new ValidationException(validationFailures);
        }

        // Proceed to the next handler if validation passes
        return await next();
    }
}
