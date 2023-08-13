using MediatR;
using Presentation.Mappers;
using Microsoft.AspNetCore.Mvc;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;
using Infrastructure.Authentication.LogIn;
using Infrastructure.Authentication.Models;
using Presentation.ApiModels.Register;
using Infrastructure.Authentication.Register;
using Presentation.Factories;
using Infrastructure.Authentication.Permissions;
using Presentation.PermissionsContainer;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : APIController
{
	private readonly ILogger<AuthenticationController> _logger;

	public AuthenticationController(ILogger<AuthenticationController> logger, ISender sender, IMapper mapper)
		: base(sender, mapper)
	{
		_logger = logger;
	}


	[HttpPost("LogIn")]
	public async Task<IActionResult> LogIn([FromBody] LogInModel model)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(Result.Failure(new Error("Model State", "Model State is not valid")));
		}

		try
		{
			Result<string> response = await _sender.Send(new LogInCommand(model));

			if (response.IsFailure)
			{
				Result result = Result.Failure(response.Error);
				return BadRequest(result);
			}
			//Thread.Sleep(1000);

			return Ok(response);
		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("Model State", exception.Message)));
		}
	}



	[HttpPost("Register")]
	[HasPermission(AuthorizationPermissions.RegisterCustomer)]
	public async Task<IActionResult> Register([FromForm] RegistrationModel model)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(Result.Failure(new Error("Model State", "Model State is not valid")));
		}

		try
		{
			Result response = await _sender.Send(new RegisterNewCustomerCommand(UserFactory.Create(model), model.Password));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(new Error("Model State", "Model State is not valid")));
			}

			return Ok(response);
		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("Model State", exception.Message)));
		}
	}
}
