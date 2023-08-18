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
using Presentation.ApiModels;
using Infrastructure.DataAccess.UserPersistence;
using Application.IdentityChecker;
using Infrastructure.ForgetPasswordHandling.ForgetPasswordServices;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : APIController
{
	private readonly ILogger<AuthenticationController> _logger;
	private readonly IUserPersistenceService _userPersistenceService;
	private readonly IExecutorIdentityProvider _identityProvider;
	private readonly IForgetPasswordService _forgetPasswordService;
	public AuthenticationController(IForgetPasswordService forgetPasswordService,IExecutorIdentityProvider identityProvider, IUserPersistenceService persistenceService, ILogger<AuthenticationController> logger, ISender sender, IMapper mapper)
		: base(sender, mapper)
	{
		_forgetPasswordService = forgetPasswordService;
		_identityProvider = identityProvider;
		_logger = logger;
		_userPersistenceService = persistenceService;
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





	[HttpPut("ChangePassword")]
	[HasPermission(AuthorizationPermissions.ReadContent)]
	public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordModel model)
	{
		try
		{
			long id = long.Parse(_identityProvider.GetExecutorId());

			await _userPersistenceService.ChangePassword(id, model.OldPassword, model.NewPassword);

			return Ok();
		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("Problem", exception.Message)));
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


	[HttpPost("ForgetPassword")]
	public async Task<IActionResult> ForgetPassword([FromForm] int serialNumber)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(Result.Failure(new Error("Model State", "Model State is not valid")));
		}

		try
		{
			Result<long> response = await _forgetPasswordService.SendCodeVIAMailAsync(serialNumber);

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
