namespace Infrastructure.Notification;
public class NotificationMessage
{
	public string MessageSubject { get; set; } = string.Empty;
	public string MessageContent { get; set; } = string.Empty;
	public DateTime? SentAt { get; set; }
}
