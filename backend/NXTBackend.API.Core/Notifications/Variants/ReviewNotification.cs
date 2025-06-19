using System.Net.Mail;
using System.Text.Json;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Evaluation.v2;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models.Shared;

namespace NXTBackend.API.Core.Notifications;


public class ReviewNotification(User notifier, Review review) : INotification
{
	public User Notifier { get; init; } = notifier;
	public Review Review { get; init; } = review;

	public static NotificationKind Descriptor => NotificationKind.Private | NotificationKind.Review | NotificationKind.Feed;

	public Domain.Entities.Notification ToDatabase()
	{
		var data = new NotificationDataDO(
			Title: "You have received a new Review",
			TextBody: $"Someone has done a review your project: {Review.UserProject.Project.Name}",
			HtmlBody: null
		);

		return new()
        {
            Descriptor = Descriptor,
            Type = nameof(ReviewNotification),
            Data = JsonSerializer.Serialize(data),
            NotifiableId = Notifier.Id,
            ResourceId = Review.Id
		};
	}

	public bool ShouldSend() => Review.State != ReviewState.Finished;

	public MailMessage? ToMail() => null;

	public string? ToText() => null;
}
