using System.Net.Mail;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Core.Notifications;

/// <summary>
/// Represents an abstract base class for notifications that can be sent through various channels.
/// </summary>
public interface INotification
{
    /// <summary>
    /// Describes the notification. This is used to describe the notification in such a way
    /// that any frontend can inteprid on how / what to display.
    ///
    /// E.g: Private | Review | AcceptOrDecline -> A private Notification regarding a review with something to accept or decline it.
    /// </summary>
    public static abstract NotificationKind Descriptor { get; }

    /// <summary>
    /// Sends this notification as an email.
    /// </summary>
    public MailMessage? ToMail();

    /// <summary>
    /// Sends this notification as a text message.
    /// </summary>
    public string? ToText();

    /// <summary>
    /// Stores this notification in the database.
    /// Must be implemented by derived classes.
    /// </summary>
    public abstract Domain.Entities.Notification ToDatabase();

    /// <summary>
    /// Determines whether this notification should be sent.
    /// </summary>
    public bool ShouldSend();

    /// <summary>
	/// Retrieves the HTML template for this notification
	/// </summary>
	public static string GetTemplate(string View)
	{
		var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", $"{View}.html");
		return File.ReadAllText(templatePath);
	}
}
