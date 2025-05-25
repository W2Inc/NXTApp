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

namespace NXTBackend.API.Core.Notifications;

/// <summary>
/// Invitation notification to user for a given user project
/// </summary>
/// <param name="to"></param>
/// <param name="project"></param>
public class ReviewNotification(User Target, Review Review) : Notification
{
	public override string View => nameof(ReviewNotification);

	public override bool ShouldSend() => true;


	public override Domain.Entities.Notification ToDatabase() => new()
	{
		NotifiableId = Target.Id,
		Type = nameof(ReviewNotification),
		Data = JsonSerializer.Serialize(new Data(
			null,
			$"You have a new review for {Review.UserProject.Project.Name}" +
			(Review.Reviewer is not null ? $" by {Review.Reviewer.DisplayName}." : ".")
		)),
	};

	public override Domain.Entities.Feed? ToFeed()
	{
		return new Domain.Entities.Feed
		{
			NotifiableId = Target.Id,
			ResourceId = Review.Id,
			Kind = FeedKind.Private | FeedKind.AcceptOrDecline | FeedKind.Review,
		};
	}
}
