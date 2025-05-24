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

namespace NXTBackend.API.Core.Notifications;

/// <summary>
/// Invitation notification to user for a given user project
/// </summary>
/// <param name="to"></param>
/// <param name="project"></param>
public class Invite(User to, UserProject project) : Notification
{
    private User User { get; init; } = to;
    private UserProject Project { get; init; } = project;

    public override MailMessage ToMail()
    {
        var mail = new MailMessage();

        mail.To.Add(User.Details?.Email ?? throw new ServiceException("No Email!"));
        mail.Subject = "Welcome to our platform!";

        // Render the view to HTML
        // TODO: Instead I may want to use Razor, but I don't wanna spend 6 years implementing that now
        mail.Body = GetTemplate()
            .Replace("{{login}}", User.Login);
        mail.IsBodyHtml = true;

        return mail;
    }

    public override bool ShouldSend()
    {
        return Project.Members.Any(m => m.UserId != User.Id);
    }

    public override bool ShouldBeDiscarded()
    {
        return Project.Members.Any(m => m.UserId != User.Id);
    }

    public override Domain.Entities.Feed? ToFeed()
    {
        return new()
        {
            Kind = FeedKind.Invite,
            ResourceId = Project.Id
        };
    }

    public override Domain.Entities.Notification ToDatabase() => new()
    {
        Type = nameof(Invite),
        NotifiableId = User.Id,
    };


    public override string View => nameof(Welcome);
}
