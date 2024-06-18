namespace NXTBackend.API.Models;

/// <summary>
/// Base response DTO (Data Transfer Objects) for all responses.
/// </summary>
public class BaseResponseDto
{
    /// <summary>
    /// Message to be returned to the client.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Errors to be returned to the client.
    /// </summary>    
    public IList<string> Errors { get; set; } = [];

    /// <summary>
    /// Success status of the operation.
    /// </summary>
    public bool Success { get; set; }
}