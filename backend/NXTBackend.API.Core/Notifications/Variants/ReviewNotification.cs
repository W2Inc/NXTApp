using System.Net.Mail;
using System.Text.Json;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models.Shared;

namespace NXTBackend.API.Core.Notifications;


public class ReviewNotification(Review review, User notifier) : INotification
{
    public static NotificationKind Descriptor => NotificationKind.Private | NotificationKind.Review | NotificationKind.Feed;

    public Domain.Entities.Notification ToDatabase()
    {
        var data = new NotificationDataDO(
            Title: "You have received a new Review",
            TextBody: $"Someone has done a review your project: {review.UserProject.Project.Name}",
            HtmlBody: null
        );

        return new()
        {
            Type = Descriptor,
            Data = JsonSerializer.Serialize(data),
            NotifiableId = notifier.Id,
        };
    }

    public bool ShouldSend() => review.State != ReviewState.Finished;

    public MailMessage? ToMail() => null;

    public string? ToText() => null;
}
