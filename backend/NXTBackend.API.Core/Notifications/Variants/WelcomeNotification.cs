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
	public User Notifier { get; init; } = notifier;

	public static NotificationKind Descriptor => NotificationKind.Private | NotificationKind.Welcome | NotificationKind.FeedOnly;

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
			Descriptor = Descriptor,
			Type = nameof(WelcomeNotification),
			Data = JsonSerializer.Serialize(data),
			NotifiableId = Notifier.Id,
		};
	}

	public MailMessage? ToMail()
	{
		return null;
		// var mail = new MailMessage();
		// if (Notifier.Details?.Email is null)
		// 	return null;

		// mail.To.Add(Notifier.Details.Email);
		// mail.Subject = "Welcome to our platform!";
		// mail.IsBodyHtml = true;
		// mail.Body = INotification
		// 	.GetTemplate(nameof(WelcomeNotification))
		// 	.Replace("{{login}}", Notifier.Login);

		// return mail;
	}

	public string? ToText() => null;
}
