using Application.IdentityChecker;
using Application.Reservations.UseCases.Cancel;
using Application.Reservations.UseCases.Create;
using Application.Reservations.UseCases.GetBetweenTwoDates;
using Application.Reservations.UseCases.GetByCustomerSerialNumber;
using Application.Reservations.UseCases.GetByDate;
using Infrastructure.Authentication.Permissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.ApiModels.Reservations;
using Presentation.Mappers;
using Presentation.PermissionsContainer;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;
using System;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReservationsController : APIController
{
	private readonly ILogger<ReservationsController> _logger;
	private readonly IExecutorIdentityProvider _identityProvider;
	public ReservationsController(ILogger<ReservationsController> logger, ISender sender, IMapper mapper, IExecutorIdentityProvider identityProvider)
		: base(sender, mapper)
	{
		_logger = logger;
		_identityProvider = identityProvider;
	}


	[HttpPost("Create")]
	[HasPermission(AuthorizationPermissions.OrderContent)]
	public async Task<IActionResult> Create([FromForm] long orderedMealId)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(Result.Failure(new Error("Model State", "Model State is not valid")));
		}

		try
		{

			long customerId = long.Parse(_identityProvider.GetExecutorId()); 
			Result<CreateReservationResponse> response = await _sender.Send(new CreateReservationCommand(customerId,orderedMealId));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(response.Error));
			}

			return Ok(response);
		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("Model State", exception.Message)));
		}
	}

	[HttpPost("Cancel")]
	[HasPermission(AuthorizationPermissions.OrderContent)]
	public async Task<IActionResult> Cancel([FromForm] long reservationId)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(Result.Failure(new Error("Model State", "Model State is not valid")));
		}
		try
		{
			Result response = await _sender.Send(new CancelReservationCommand(reservationId));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(response.Error));
			}

			return Ok(response);
		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("Model State", exception.Message)));
		}
	}

	[HttpGet("GetReservationsByDate")]
	[HasPermission(AuthorizationPermissions.ReadSystemInfo)]
	public async Task<IActionResult> GetReservationsByDate(string date)
	{
		try
		{
			DateOnly dateFilter = DateOnly.Parse(date);

			var response = await _sender.Send(new GetReservationsByDateQuery(dateFilter));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(response.Error));
			}
			return Ok(response.Value);

		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("", exception.Message)));
		}
	}

	[HttpGet("GetReservationsBetweenTwoDates")]
	[HasPermission(AuthorizationPermissions.ReadSystemInfo)]
	public async Task<IActionResult> GetReservationsBetweenTwoDates(string startDate, string endDate)
	{
		try
		{
			DateOnly start = DateOnly.Parse(startDate);
			DateOnly end = DateOnly.Parse(endDate);

			if(start > end)
			{
				throw new Exception("start > end");
			}

			var response = await _sender.Send(new GetReservationsBetweenTwoDatesQuery(start,end));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(response.Error));
			}
			return Ok(response.Value);

		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("", exception.Message)));
		}
	}




	[HttpGet("GetReservationsByCustomerSerialNumber")]
	[HasPermission(AuthorizationPermissions.ReadSystemInfo)]
	public async Task<IActionResult> GetReservationsByCustomerSerialNumber(int serialNumber)
	{
		try
		{
			var response = await _sender.Send(new GetReservationsByCustomerSerialNumberQuery(serialNumber));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(response.Error));
			}
			return Ok(response.Value);

		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("", exception.Message)));
		}
	}
}
