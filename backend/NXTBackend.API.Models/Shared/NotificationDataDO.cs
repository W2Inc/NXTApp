namespace NXTBackend.API.Models.Shared;

/// <summary>
/// Data transfer object representing serialized notification data for processing.
/// </summary>
public record NotificationDataDO(
    string Title,
    string? HtmlBody = null,
    string? TextBody = null
);
