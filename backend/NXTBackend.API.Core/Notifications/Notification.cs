using System.Net.Mail;

namespace NXTBackend.API.Core.Notifications;

/// <summary>
/// Represents an abstract base class for notifications that can be sent through various channels.
/// </summary>
public abstract class Notification()
{
    /// <summary>
    /// Gets the view name for this notification.
    /// Can be overridden by implementing classes.
    /// </summary>
    public virtual string View => nameof(Notification);

    /// <summary>
    /// Sends this notification as an email.
    /// </summary>
    public virtual MailMessage ToMail() => new();

    /// <summary>
    /// Sends this notification as a text message.
    /// </summary>
    public virtual string ToText() => string.Empty;

    /// <summary>
    /// Stores this notification in the database using the provided service.
    /// Must be implemented by derived classes.
    /// </summary>
    public abstract Domain.Entities.Notification ToDatabase();

    /// <summary>
    /// Determines whether this notification should be sent.
    /// </summary>
    public virtual bool ShouldSend() => true;

    /// <summary>
    /// Retrieves the View (HTML) of this notification
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string GetTemplate()
    {
        // string cacheKey = $"template_{View}";
        // var cachedTemplate = _cache.GetString(cacheKey);
        // if (!string.IsNullOrEmpty(cachedTemplate))
        //     return cachedTemplate;

        // Cache miss - read from file
        string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", $"{View}.html");
        string template = File.ReadAllText(templatePath);
        // var cacheOptions = new DistributedCacheEntryOptions
        // {
        //     AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        // };

        // _cache.SetString(cacheKey, template, cacheOptions);
        return template;
    }
}
