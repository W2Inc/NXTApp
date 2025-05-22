using System.Diagnostics.CodeAnalysis;
using NXTBackend.API.Core.Notifications;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Core.Services.Interface.Queues;

public interface INotificationQueue
{
	public record NotificationEntry(User User, Notification Notification);

	/// <summary>
	/// Gets the current number of notifications waiting in the queue
	/// </summary>
	int QueueLength { get; }

	/// <summary>
	/// Adds a notification to the queue for processing
	/// </summary>
	/// <param name="notification">The notification to enqueue</param>
	/// <returns>The ID assigned to the notification</returns>
	void Enqueue(User user, Notification notification);

	/// <summary>
	/// Attempts to dequeue a notification from the queue.
	/// </summary>
	/// <param name="result">When this method returns, contains the notification that was dequeued if the operation was successful, or the default value if the operation failed.</param>
	/// <returns>true if a notification was successfully dequeued; otherwise, false.</returns>
	bool TryDequeue(out NotificationEntry result);

	/// <summary>
	/// Attempts to clear all pending notifications
	/// </summary>
	void Clear();
}