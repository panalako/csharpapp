namespace CSharpApp.Core.Dtos.ValidationDtos;

public class ValidationFailureResponse
{
    public required IEnumerable<ValidationResponse> Errors { get; init; }
}