using NXTBackend.API.Core.Notifications;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Services.Interface.Queues;
using NXTBackend.API.Domain.Enums;

// using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Jobs.Interface;
using Quartz;
using Resend;


namespace NXTBackend.API.Jobs;


[DisallowConcurrentExecution]
public class DispatchNotificationsJob2(
	DatabaseContext ctx,
	ILogger<DispatchNotificationsJob> logger,
	INotificationQueue queue,
	IResend resend,
	IFeedService feedService
) : IScheduledJob
{
	public static string? Schedule => "0 0/1 * ? * *";
	public static string Identity => nameof(DispatchNotificationsJob);

	public async Task Execute(IJobExecutionContext context)
	{
		logger.LogInformation("Dispatching notifications...");
		if (queue.QueueLength == 0)
		{
			logger.LogInformation("No notifications to process");
			return;
		}

		while (queue.TryDequeue(out var entry))
		{
			if (entry is null)
				continue;

			var (user, notification) = entry;
			logger.LogInformation("Processed notification for {NotifiableId}", notification.NotifiableId);
			if (notification is null || user is null || !notification.ShouldSend())
			{
				logger.LogInformation("Notification for {NotifiableId} was discarded", notification?.NotifiableId);
				continue;
			}

			NotificationState state;
			try
			{
				await Task.WhenAll([
					ProcessToSMS(notification),
					ProcessToEmail(notification),
					ProcessToFeed(notification),
				]);

				state = NotificationState.Dispatched;
			}
			catch (Exception e)
			{
				logger.LogError(e, "Failed to process notification for {NotifiableId}", notification.NotifiableId);
				state = NotificationState.Failed;
			}

			await ProcessToDatabase(notification, state);
		}

		logger.LogInformation("All notifications processed, saving to database...");
		await ctx.SaveChangesAsync(context.CancellationToken);
	}

	private Task ProcessToSMS(Notification notification)
	{
		var text = notification.ToText();
		if (string.IsNullOrEmpty(text))
			return Task.CompletedTask;

		// TODO: Implement SMS processing logic here

		return Task.CompletedTask;
	}

	private Task ProcessToEmail(Notification notification)
	{
		var mail = notification.ToMail();
		if (mail is null)
			return Task.CompletedTask;

		// Implement email processing logic here
		// This could involve sending an email using a service like SendGrid or similar
		// For example:
		// var emailService = new EmailService();
		// await emailService.SendEmailAsync(notification.Data, notification.NotifiableId);

		logger.LogInformation("Processed email notification for {NotifiableId}", notification.NotifiableId);
		return Task.CompletedTask;
	}

	private async Task ProcessToFeed(Notification notification)
	{
		logger.LogInformation("Processing feed for notification {NotifiableId}", notification.NotifiableId);
		var feed = notification.ToFeed();
		if (feed is null)
		{
			logger.LogInformation("No feed to process for notification {NotifiableId}", notification.NotifiableId);
			return;
		}

		// Publish the feed to the feed service
		await ctx.Feeds.AddAsync(feed);
		return;
	}

	private async Task ProcessToDatabase(Notification notification, NotificationState state)
	{
		var dbNotification = notification.ToDatabase();
		dbNotification.State = state;
		await ctx.Notifications.AddAsync(dbNotification);
	}
}
