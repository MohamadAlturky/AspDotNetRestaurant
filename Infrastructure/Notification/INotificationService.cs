namespace Infrastructure.Notification;

public interface INotificationService
{
	Task SendToAllAsync(NotificationMessage notificationMessage);
}
