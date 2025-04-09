using System.Text.Json.Serialization;

namespace NXTBackend.API.Models;

/// <summary>
/// Base response DTO (Data Transfer Objects) for all responses.
/// </summary>
public class BaseResponseDto
{
    /// <summary>
    /// Message to be returned to the client.
    /// </summary>
    /// <example>Something went wrong</example>
    [JsonPropertyName(nameof(Message))]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Success status of the operation.
    /// </summary>
    /// <example>false</example>
    [JsonPropertyName(nameof(Success))]
    public bool Success { get; set; } = true;
}

public class ErrorResponseDto : BaseResponseDto
{
    public ErrorResponseDto(string message = "Internal server error")
    {
        Message = message;
        Success = false;
    }
}
