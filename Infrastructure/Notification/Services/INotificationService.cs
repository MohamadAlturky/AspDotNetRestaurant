using Infrastructure.Notification.Model;

namespace Infrastructure.Notification.Services;

public interface INotificationService
{
	Task SendToAllAsync(NotificationMessage notificationMessage);
}
