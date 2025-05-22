// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Jobs.Interface;
using NXTBackend.API.Models.Shared;
using Quartz;
using Resend;

// ============================================================================

namespace NXTBackend.API.Jobs;

/// <summary>
/// A job that computes the composition of a project's review.
/// Essentially projects are made up of multiple reviews.
/// </summary>
/// <param name="ctx">Database</param>
/// <param name="logger">Logger</param>
public class DispatchNotificationsJob(ILogger<DispatchNotificationsJob> logger, DatabaseContext ctx, IResend resend) : IScheduledJob
{
    /// <inheritdoc />
    public static string Identity => nameof(DispatchNotificationsJob);

    /// <inheritdoc />
    public static string? Schedule => "0 0/5 * ? * *";


    /// <inheritdoc />
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("{Job} is starting", Identity);
        var pendingNotifications = await ctx.Notifications
            .Where(n => n.State != NotificationState.Dispatched)
            .ToListAsync(context.CancellationToken);

        if (pendingNotifications.Count == 0)
        {
            logger.LogInformation("{Job} finished early. Nothing to dispatch", Identity);
            return;
        }

        logger.LogInformation("Found {Count} notifications to dispatch", pendingNotifications.Count);
        var successfulIds = new List<Guid>();
        var failedIds = new List<Guid>();

        foreach (var notification in pendingNotifications)
        {
            try
            {
                logger.LogInformation("Processing notification ID: {Id}", notification.Id);
                var data = JsonSerializer.Deserialize<NotificationDataDO>(notification.Data);

                if (data?.Text is not null)
                {
                    // Process text notification
                }

                // TODO: Implement actual email sending using the resend service
                // Example: await resend.Emails.SendAsync(new EmailMessage { ... });

                successfulIds.Add(notification.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process notification ID: {Id}", notification.Id);
                failedIds.Add(notification.Id);
            }
        }

        if (successfulIds.Count != 0)
        {
            await ctx.Notifications
                .Where(n => successfulIds.Contains(n.Id))
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(n => n.State, NotificationState.Dispatched),
                    context.CancellationToken);

            logger.LogInformation("Successfully dispatched {Count} notifications", successfulIds.Count);
        }
        if (failedIds.Count != 0)
        {
            await ctx.Notifications
                .Where(n => failedIds.Contains(n.Id))
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(n => n.State, NotificationState.Failed),
                    context.CancellationToken);

            logger.LogWarning("{Count} notifications failed to dispatch", failedIds.Count);
        }
    }
}
