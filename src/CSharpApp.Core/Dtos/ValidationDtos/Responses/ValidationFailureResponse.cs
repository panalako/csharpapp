namespace CSharpApp.Core.Dtos.ValidationDtos.Responses;

public class ValidationFailureResponse
{
    public required IEnumerable<ValidationResponse> Errors { get; init; }
}