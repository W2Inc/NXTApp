using NXTBackend.API.Domain.Notifications.Base;

namespace NXTBackend.API.Domain.Notifications;

public class WelcomeNotification : Notification
{
	public override Task ToMail()
	{
		// Implement the logic to send the notification as an email
		throw new NotImplementedException();
	}
	

	public override bool ShouldSend()
	{
		return true;
	}
}