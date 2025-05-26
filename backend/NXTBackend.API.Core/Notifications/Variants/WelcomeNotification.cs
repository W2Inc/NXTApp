using System.Net.Mail;
using System.Threading.Tasks;
using NXTBackend.API.Core.Services;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Users;
using System.Web;
using NXTBackend.API.Core.Utils;
using Microsoft.AspNetCore.Http;
using System.Net;
using NXTBackend.API.Domain.Enums;
using System.Text.Json;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Models.Shared;

namespace NXTBackend.API.Core.Notifications.Variants;

public class WelcomeNotification(User notifier) : INotification
{
    public static NotificationKind Descriptor => NotificationKind.Private | NotificationKind.Welcome;

    public bool ShouldSend() => true;

    public Notification ToDatabase()
    {
        var data = new NotificationDataDO(
            Title: "You have received a new Review",
            TextBody: null,
            HtmlBody: null
        );

        return new()
        {
            Type = Descriptor,
            Data = JsonSerializer.Serialize(data),
            NotifiableId = notifier.Id,
        };
    }

    public MailMessage? ToMail()
    {
        var mail = new MailMessage();
        if (notifier.Details?.Email is null)
            return null;

        mail.To.Add(notifier.Details.Email);
        mail.Subject = "Welcome to our platform!";
        mail.IsBodyHtml = true;
        mail.Body = INotification
            .GetTemplate(nameof(WelcomeNotification))
            .Replace("{{login}}", notifier.Login);

        return mail;
    }

    public string? ToText()
    {
        throw new NotImplementedException();
    }

    // public override MailMessage ToMail()
    // {
    //     var mail = new MailMessage();

    //     mail.To.Add(to.Details?.Email ?? throw new ServiceException("No Email!"));
    //     mail.Subject = "Welcome to our platform!";

    //     // Render the view to HTML
    //     // TODO: Instead I may want to use Razor, but I don't wanna spend 6 years implementing that now
    //     mail.Body = GetTemplate()
    //         .Replace("{{login}}", to.Login);
    //     mail.IsBodyHtml = true;

    //     return mail;
    // }

    // public override bool ShouldSend()
    // {
    //     return true;
    // }

    // public override Domain.Entities.Notification ToDatabase() => new()
    // {
    // 	Type = nameof(Welcome),
    // 	NotifiableId = to.Id,
    // 	Data = JsonSerializer.Serialize(new Data(
    // 		null,
    // 		"Welcome to our platform! We are excited to have you on board. If you have any questions, feel free to reach out."
    // 	))
    // };

    // public override Domain.Entities.Feed? ToFeed()
    // {
    // 	return new()
    // 	{
    // 		Kind = FeedKind.Default | FeedKind.Private | FeedKind.Welcome,
    // 		NotifiableId = to.Id
    // 	};
    // }

    // // private User User { get; init; } = to;

    // public override string View => nameof(Welcome);
}
