using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Entities.Event;

namespace NXTBackend.API.Models.Responses;

/// <summary>
/// Request for setting up a git repository
/// </summary>
public class EventGetResponseDto : BaseResponseDto
{
    [JsonPropertyName(nameof(Event))]
    public Notification Event { get; set; } = null!;
}
