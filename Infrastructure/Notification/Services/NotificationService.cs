﻿using Infrastructure.Notification.Hubs;
using Infrastructure.Notification.Model;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Notification.Services;

public class NotificationService : INotificationService
{
	private readonly IHubContext<NotificationsHub> _notificationHubContext;


	public NotificationService(IHubContext<NotificationsHub> hubContext)
	{
		_notificationHubContext = hubContext;
	}

	public async Task SendToAllAsync(NotificationMessage notificationMessage)
	{
		await _notificationHubContext.Clients.All.SendAsync("RecieveMessage",
			notificationMessage.MessageSubject,
			notificationMessage.MessageContent,
			notificationMessage.SentAt);
	}
}