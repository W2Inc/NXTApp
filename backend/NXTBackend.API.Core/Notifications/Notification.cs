using System.Net.Mail;
using System.Text.Json.Serialization;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Core.Notifications;

/// <summary>
/// Represents an abstract base class for notifications that can be sent through various channels.
/// </summary>
public abstract class Notification
{
	public record Data(
		[property: JsonPropertyName("html")]
		[property: JsonInclude]
		string? HtmlBody = null,

		[property: JsonInclude]
		[property: JsonPropertyName("text")]
		string? TextBody = null
	);
	
    /// <summary>
	/// The ID of the user who should receive this notification
	/// </summary>
	public Guid NotifiableId { get; init; }

    /// <summary>
    /// Gets the view name for this notification.
    /// Can be overridden by implementing classes.
    /// </summary>
    public virtual string View => nameof(Notification);

    /// <summary>
    /// Sends this notification as an email.
    /// </summary>
    public virtual MailMessage? ToMail() => null;

    /// <summary>
    /// Sends this notification as a text message.
    /// </summary>
    public virtual string? ToText() => null;

    /// <summary>
    /// If defined will create a feed response.
    /// </summary>
    /// <returns></returns>
    public virtual Feed? ToFeed() => null;

    /// <summary>
    /// Stores this notification in the database.
    /// Must be implemented by derived classes.
    /// </summary>
    public abstract Domain.Entities.Notification ToDatabase();

    /// <summary>
    /// Determines whether this notification should be sent.
    /// </summary>
    public virtual bool ShouldSend() => true;

    /// <summary>
	/// Retrieves the HTML template for this notification
	/// </summary>
	public string GetTemplate()
	{
		string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", $"{View}.html");
		return File.ReadAllText(templatePath);
	}
}
