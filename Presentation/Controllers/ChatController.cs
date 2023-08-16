using Application.IdentityChecker;
using Infrastructure.Authentication.Permissions;
using Infrastructure.Notification;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.ApiModels;
using Presentation.Mappers;
using Presentation.PermissionsContainer;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChatController : APIController
{
	private readonly IHubContext<NotificationsHub> _notificationsHub;

	public ChatController(ISender sender, IMapper mapper, IHubContext<NotificationsHub> notificationsHub)
		: base(sender, mapper)
	{
		_notificationsHub = notificationsHub;
	}

	[HttpPost("SendToAll")]
	[HasPermission(AuthorizationPermissions.CreateContent)]
	public async Task<IActionResult> SendToAll([FromForm] BroadCastRequest model)
	{
		try
		{
			await _notificationsHub.Clients.All.SendAsync("RecieveMessage", model.Title + model.Content);
			return Ok();
		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("Problem", exception.Message)));
		}
	}
}
