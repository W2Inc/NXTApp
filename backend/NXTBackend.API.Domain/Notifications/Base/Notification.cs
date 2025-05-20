using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Services;

namespace NXTBackend.API.Domain.Notifications.Base;

/// <summary>
/// Represents an abstract base class for notifications that can be sent through various channels.
/// </summary>
public abstract class Notification
{
    /// <summary>
    /// Gets the type of this notification
    /// </summary>
    public abstract NotificationKind Kind { get; }
    
    /// <summary>
    /// Gets the message content of this notification
    /// </summary>
    public abstract string Message { get; }
    
    /// <summary>
    /// Gets an optional resource ID related to this notification
    /// </summary>
    public virtual Guid? ResourceId => null;

    /// <summary>
    /// Sends this notification as an email.
    /// </summary>
    public virtual Task ToMail() => throw new NotImplementedException();

    /// <summary>
    /// Sends this notification as an SMS message.
    /// </summary>
    public virtual Task ToSMS() => throw new NotImplementedException();

    /// <summary>
    /// Stores this notification in the database using the provided service.
    /// </summary>
    public virtual async Task ToDatabase(NotificationService notificationService)
    {
        if (notificationService == null)
        {
            throw new ArgumentNullException(nameof(notificationService));
        }
        
        await notificationService.SaveToDatabaseAsync(Message, Kind, ResourceId);
    }

    /// <summary>
    /// Determines whether this notification should be sent.
    /// </summary>
    public virtual bool ShouldSend() => true;
}