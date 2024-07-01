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
    [JsonPropertyName(nameof(Message))]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Errors to be returned to the client.
    /// </summary>    
    [JsonPropertyName(nameof(Errors))]
    public IList<string> Errors { get; set; } = [];

    /// <summary>
    /// Success status of the operation.
    /// </summary>
    [JsonPropertyName(nameof(Success))]
    public bool Success { get; set; }
}