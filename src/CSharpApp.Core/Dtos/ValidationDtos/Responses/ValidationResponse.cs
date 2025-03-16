namespace CSharpApp.Core.Dtos.ValidationDtos.Responses;

public record ValidationResponse
{
    public required string PropertyName { get; init; }

    public required string Message { get; init; }
}