using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using NXTBackend.API.Core.Notifications;
using NXTBackend.API.Core.Services.Interface.Queues;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Core.Services.Implementation.Queues;

/// <summary>
/// An in-memory implementation of a notification queue.
/// This class is used to manage notifications in a queue for processing.
/// 
/// It provides methods to enqueue notifications, peek at the queue, cancel notifications,
/// and clear the queue.
/// 
/// This implementation is prone to data loss if the application is restarted.
/// 
/// Yeah we should probably use smth like Redis or RabbitMQ for this.
/// But for now this is a simple in-memory queue.
/// </summary>
public class InMemoryNotificationQueue(ILogger<InMemoryNotificationQueue> logger) : INotificationQueue
{
	private readonly ConcurrentQueue<INotificationQueue.NotificationEntry> _notifications = new();

	public int QueueLength => _notifications.Count;

	public void Enqueue(User user, Notification notification)
	{
		logger.LogInformation("Enqueuing notification for user {userId}", user.Id);
		_notifications.Enqueue(new(user, notification));

		logger.LogInformation("Notification enqueued");
		logger.LogInformation("Queue length: {queueLength}", QueueLength);
	}

	public bool TryDequeue(out INotificationQueue.NotificationEntry result)
	{
		logger.LogInformation("Attempting to dequeue notification");
		return _notifications.TryDequeue(out result!);
	}

	public void Clear() => _notifications.Clear();
}