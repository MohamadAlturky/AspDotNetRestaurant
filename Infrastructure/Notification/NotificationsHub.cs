using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Notification;
public class NotificationsHub : Hub
{
	//public override async Task OnConnectedAsync()
	//{
	//	await Clients.All.SendAsync("RecieveMessage", Context.ConnectionId);
	//}
	public async Task SendMessage(string title, string content)
	{
		await Clients.All.SendAsync("RecieveMessage", title + content);
	}
}
