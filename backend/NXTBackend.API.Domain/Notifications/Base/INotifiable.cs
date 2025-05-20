// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================


namespace NXTBackend.API.Domain.Notifications.Base;

public interface INotifiable
{
	public Task NotifyAsync(Notification notification);
}