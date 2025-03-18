namespace CSharpApp.Core.Dtos.AuthDtos;

public class AuthendiationFailureDto
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }

}