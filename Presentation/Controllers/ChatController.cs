﻿using Application.IdentityChecker;
using Infrastructure.Authentication.Permissions;
using Infrastructure.Notification;
using Infrastructure.Notification.Hubs;
using Infrastructure.Notification.Model;
using Infrastructure.Notification.Services;
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
	private readonly INotificationService _notificationService;

	public ChatController(ISender sender, IMapper mapper, INotificationService notificationService)
		: base(sender, mapper)
	{
		_notificationService = notificationService;
	}

	[HttpPost("SendToAll")]
	[HasPermission(AuthorizationPermissions.CreateSystemInformation)]
	public async Task<IActionResult> SendToAll([FromForm] BroadCastRequest model)
	{
		try
		{
			await _notificationService.SendToAllAsync(new NotificationMessage()
			{
				MessageContent=model.Content,
				MessageSubject=model.Title,
				SentAt=DateTime.UtcNow
			});
			return Ok();
		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("Problem", exception.Message)));
		}
	}
}
