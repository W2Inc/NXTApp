using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Services.Interface.Queues;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Jobs.Interface;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Resend;
using Microsoft.Extensions.Caching.Distributed;

namespace NXTBackend.API.Jobs;

[DisallowConcurrentExecution]
public class DispatchNotificationsJob(
	DatabaseContext ctx,
	ILogger<DispatchNotificationsJob> logger,
	INotificationQueue queue,
	IDistributedCache cache,
	IResend resend
) : IScheduledJob
{

	public static string Identity => nameof(DispatchNotificationsJob);

	public static string? Schedule => "0 0/1 * ? * *";

	public async Task Execute(IJobExecutionContext context)
	{
		logger.LogInformation("{Job} is starting", Identity);


		// Early return if queue is empty
		if (queue.QueueLength == 0)
		{
			logger.LogInformation("{Job} finished early. No notifications in queue: {Length}", Identity, queue.QueueLength);
			return;
		}

		var emails = new List<EmailMessage>(queue.QueueLength);
		while (queue.TryDequeue(out var notificationEntry))
		{
			var (user, notification) = notificationEntry;
			logger.LogInformation("Processing notification for user {userId}", user.Id);
			if (!notification.ShouldSend())
			{
				if (notification.ShouldBeDiscarded())
				{
					logger.LogDebug("Notification will be discarded");
					return;
				}

				queue.Enqueue(user, notification);
				logger.LogDebug("Notification will be re-queued");
				continue;
			}

			var mail = notification.ToMail();
			if (mail is not null)
			{
				emails.Add(new EmailMessage
				{
					HtmlBody = mail.Body,
					Subject = mail.Subject,
					To = mail.To.Select(addr => addr.Address).ToArray()
				});
			}

			var text = notification.ToText();
			if (text is not null)
			{
				// Here you would implement the actual SMS sending logic
				// For example, using Twilio or another SMS service
			}

			await ctx.Notifications.AddAsync(notification.ToDatabase(), context.CancellationToken);
		}

		// Send all emails in a single batch
		if (emails.Count > 0)
		{
			logger.LogInformation("Sending {count} email notifications in batch", emails.Count);
			// var response = await resend.EmailBatchAsync([.. emails], context.CancellationToken);
		}
		
		await ctx.SaveChangesAsync(context.CancellationToken);
	}
}