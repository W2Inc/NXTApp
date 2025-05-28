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
using NXTBackend.API.Core.Notifications.Variants;
using NXTBackend.API.Models.Shared;

namespace NXTBackend.API.Core.Notifications;

/// <summary>
/// Invitation notification to user for a given user project
/// </summary>
/// <param name="to"></param>
/// <param name="project"></param>
public class InviteNotification(User notifier, UserProject userProject) : INotification
{
	public static NotificationKind Descriptor => NotificationKind.AcceptOrDecline | NotificationKind.Project | NotificationKind.Viewable;

	public bool ShouldSend() => true;

	public Domain.Entities.Notification ToDatabase() => new()
	{
		Descriptor = Descriptor,
		NotifiableId = notifier.Id,
		Type = nameof(InviteNotification),
		Data = JsonSerializer.Serialize(new NotificationDataDO(
			Title: $"You have been invited to join the project",
			HtmlBody: $"<p>You have been invited to join the project: {userProject.Project.Name}</p>.",
			TextBody: $"You have been invited to join the project."
		)),
	};

	public MailMessage? ToMail() => null;

	public string? ToText() => null;
}
