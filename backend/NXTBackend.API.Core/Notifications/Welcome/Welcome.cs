using System.Net.Mail;
using System.Threading.Tasks;
using NXTBackend.API.Core.Services;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities.Users;
using System.Web;
using NXTBackend.API.Core.Utils;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace NXTBackend.API.Core.Notifications.Welcome;

public class Welcome : Notification
{
    public override string View => nameof(Welcome);
	
	private readonly User user;

	public Welcome(User to)
	{
		user = to;
	}

	public override MailMessage ToMail()
	{
		var mail = new MailMessage();

		mail.To.Add(user.Details?.Email ?? throw new ServiceException("No Email!"));
		mail.Subject = "Welcome to our platform!";

		// Render the view to HTML
		// TODO: Instead I may want to use Razor, but I don't wanna spend 6 years implementing that now
		mail.Body = GetTemplate()
			.Replace("{{login}}", user.Login);
		mail.IsBodyHtml = true;

		return mail;
	}

    public override bool ShouldSend()
    {
        return true;
    }

    public override Domain.Entities.Notification ToDatabase()
    {
        return new Domain.Entities.Notification
		{
			Type = nameof(Welcome),
		};
    }
}
