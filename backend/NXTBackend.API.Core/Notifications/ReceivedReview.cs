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
using NXTBackend.API.Domain.Entities.Evaluation;

namespace NXTBackend.API.Core.Notifications;

/// <summary>
/// Invitation notification to user for a given user project
/// </summary>
/// <param name="to"></param>
/// <param name="project"></param>
public class ReceivedReview(User Target, Review Review) : Notification
{
    public override Domain.Entities.Notification ToDatabase() => new()
    {
        Type = nameof(ReceivedReview),
        NotifiableId = Target.Id,
    };

    public override bool ShouldSend()
    {
        // TODO: Check user preference for notifications.
        return true;
    }

    public override string View => nameof(ReceivedReview);
}
