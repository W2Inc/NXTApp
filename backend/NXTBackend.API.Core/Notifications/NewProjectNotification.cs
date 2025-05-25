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
using System.Text.Json;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Core.Notifications;

/// <summary>
/// Invitation notification to user for a given user project
/// </summary>
/// <param name="to"></param>
/// <param name="project"></param>
public class NewProjectNotification(Project Target) : Notification
{
	public override bool ShouldSend() => Target.Public;

	public override Domain.Entities.Notification ToDatabase() => new()
	{
		NotifiableId = Target.Id,
		Type = nameof(NewProjectNotification),
		Data = JsonSerializer.Serialize(new Data(
			null,
			$"A new project \"{Target.Name}\" has been created."
		)),
	};

	public override Feed? ToFeed()
	{
		return new()
		{
			ResourceId = Target.Id,
			Kind = FeedKind.Global | FeedKind.Project | FeedKind.Viewable,
		};
	}
}
