namespace CSharpApp.Core.Dtos.ValidationDtos;

public record ValidationResponse
{
    public required string PropertyName { get; init; }

    public required string Message { get; init; }
}