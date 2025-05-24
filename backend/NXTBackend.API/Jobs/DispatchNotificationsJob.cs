using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Services.Interface.Queues;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Jobs.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Quartz;
using Resend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NXTBackend.API.Core.Notifications.Welcome;

namespace NXTBackend.API.Jobs;

[DisallowConcurrentExecution]
public class DispatchNotificationsJob : IScheduledJob
{
    private readonly DatabaseContext _ctx;
    private readonly ILogger<DispatchNotificationsJob> _logger;
    private readonly INotificationQueue _queue;
    private readonly IResend _resend;
    private readonly IFeedService _feedService;

    // Configuration settings
    private const int MaxBatchSize = 50;
    private const int NotificationTtlMinutes = 10;

    public DispatchNotificationsJob(
        DatabaseContext ctx,
        ILogger<DispatchNotificationsJob> logger,
        INotificationQueue queue,
        IFeedService feedService,
        IResend resend)
    {
        _ctx = ctx;
        _logger = logger;
        _queue = queue;
        _resend = resend;
        _feedService = feedService;
    }

    public static string Identity => nameof(DispatchNotificationsJob);
    public static string? Schedule => "0 0/1 * ? * *";

    public async Task Execute(IJobExecutionContext context)
    {
        // 1. Process in-memory queue first
        await ProcessQueuedNotifications(context.CancellationToken);
        // await ProcessFailedNotifications(context.CancellationToken);

        _logger.LogInformation("{Job} completed successfully", Identity);
    }

    private async Task ProcessQueuedNotifications(CancellationToken cancellationToken)
    {
        if (_queue.QueueLength == 0)
        {
            _logger.LogInformation("{Job} has no queued notifications to process", Identity);
            return;
        }

        _logger.LogInformation("Processing {count} queued notifications", _queue.QueueLength);

        var emailBatch = new List<EmailMessage>();
        var notificationsToSave = new List<Notification>();

        // Process queue in a single pass
        while (_queue.TryDequeue(out var entry))
        {
            var (user, notification) = entry;
            if (notification.ShouldBeDiscarded())
            {
                _logger.LogDebug("Discarding outdated notification for user {userId}", user.Id);

                var dbNotification = notification.ToDatabase();
                dbNotification.State = NotificationState.Failed;
                notificationsToSave.Add(dbNotification);
                continue;
            }

            // Check if notification is ready to be sent
            if (!notification.ShouldSend())
            {
                _logger.LogDebug("Notification for user {userId} not ready to send, re-queuing", user.Id);
                _queue.Enqueue(user, notification);
                continue;
            }

            try
            {
                // Process notification based on type
                var dbNotification = notification.ToDatabase();

                // Prepare email if applicable
                var email = notification.ToMail();
                if (email is not null)
                {
                    emailBatch.Add(new EmailMessage
                    {
                        HtmlBody = email.Body,
                        Subject = email.Subject,
                        To = email.To.Select(addr => addr.Address).ToArray()
                    });
                }

                // Prepare SMS if applicable (commented out as implementation may vary)
                var text = notification.ToText();
                if (text is not null)
                {
                    // SMS sending logic would go here
                }

                var feed = notification.ToFeed();
                if (feed is not null)
                    await _feedService.CreateAsync(feed);

                // Mark as ready to be saved
                dbNotification.State = NotificationState.Dispatched;
                notificationsToSave.Add(dbNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing notification for user {userId}", user.Id);

                var failedNotification = notification.ToDatabase();
                failedNotification.State = NotificationState.Failed;
                // failedNotification.ErrorMessage = ex.Message;
                notificationsToSave.Add(failedNotification);
            }
        }

        // Process emails in a batch if any
        if (emailBatch.Count > 0)
        {
            try
            {
                _logger.LogInformation("Sending batch of {count} emails", emailBatch.Count);
                // Uncomment when ready to use
                // var response = await _resend.EmailBatchAsync(emailBatch.ToArray(), cancellationToken);

                // If email sending fails, update the corresponding notifications
                // This would need additional tracking to map emails back to notifications
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email batch");
                // Mark all email notifications as failed
                foreach (var notification in notificationsToSave.Where(n => n.State == NotificationState.Dispatched))
                {
                    notification.State = NotificationState.Failed;
                    // notification.ErrorMessage = "Email batch sending failed: " + ex.Message;
                }
            }
        }

        // Save all notifications in a single database call
        if (notificationsToSave.Count > 0)
        {
            _logger.LogInformation("Saving {count} notifications to database", notificationsToSave.Count);
            await _ctx.Notifications.AddRangeAsync(notificationsToSave, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
        }
    }

    // TODO: Figure this out, without reflection to *rebuild* the notification.
    // ToDatabase needs to have a nice way for us to be able to re-construct the notification
    private async Task ProcessFailedNotifications(CancellationToken cancellationToken)
    {
        // Get failed notifications that aren't too old
        var cutoffTime = DateTime.UtcNow.AddMinutes(-NotificationTtlMinutes);

        // Single query to get processable failed notifications
        var failedNotifications = await _ctx.Notifications
            .Where(n => n.State == NotificationState.Failed)
            .Where(n => n.CreatedAt > cutoffTime) // Only retry if not too old
            .OrderBy(n => n.CreatedAt)
            .Take(MaxBatchSize)
            .ToListAsync(cancellationToken);

        if (failedNotifications.Count == 0)
        {
            return;
        }

        _logger.LogInformation("Retrying {count} failed notifications", failedNotifications.Count);

        // Process each failed notification
        foreach (var notification in failedNotifications)
        {
            try
            {
                // Here you would implement retry logic for the failed notification
                // This would depend on your notification implementation details

                // For this example, we'll just mark it as dispatched
                notification.State = NotificationState.Dispatched;
                // notification.ErrorMessage = null;
                notification.UpdatedAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retry notification {id}", notification.Id);
                // notification.ErrorMessage = ex.Message;
                notification.UpdatedAt = DateTime.UtcNow;
                // Still failed, but we updated the error message and timestamp
            }
        }

        // Update all notifications in a single call
        await _ctx.SaveChangesAsync(cancellationToken);
    }
}
