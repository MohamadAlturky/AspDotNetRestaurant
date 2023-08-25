using Application.Customers.UseCases.DecreaseCustomerBalance;
using Application.Customers.UseCases.GetAccountTransactions;
using Application.Customers.UseCases.GetAll;
using Application.Customers.UseCases.GetSumOfCustomersBalances;
using Application.IdentityChecker;
using Application.UseCases.Customers.GetByFilter;
using Application.UseCases.Customers.GetPage;
using Application.UseCases.Customers.IncreaseCustomerBalance;
using Domain.Customers.Aggregate;
using Infrastructure.Authentication.Permissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.ApiModels.Customers;
using Presentation.Mappers;
using Presentation.PermissionsContainer;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;

namespace Presentation.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CustomersController : APIController
{
	private readonly ILogger<CustomersController> _logger;
	private readonly IExecutorIdentityProvider _executorIdentityProvider;
	public CustomersController(IExecutorIdentityProvider executorIdentityProvider,ISender sender, IMapper mapper, ILogger<CustomersController> logger) : base(sender, mapper)
	{
		_logger = logger;
		_executorIdentityProvider =executorIdentityProvider;
	}

	//[HttpPost("Create")]
	//public async Task<IActionResult> Create([FromBody] CustomerDTO customer)
	//{
	//	if (!ModelState.IsValid)
	//	{
	//		return BadRequest(Result.Failure(new Error("Model State", "Model State is not valid")));
	//	}

	//	try
	//	{
	//		Result response = await _sender.Send(new CreateCustomerCommand(_mapper.Map(customer)));

	//		if (response.IsFailure)
	//		{
	//			return BadRequest(response);
	//		}
	//		Result<Customer> insertedCustomer = await _sender.Send(new GetCustomerBySerialNumberQuery(customer.SerialNumber));

	//		var registerResponse = await _sender.Send(new RegisterNewCustomerCommand(insertedCustomer.Value, customer.Password));

	//		if (registerResponse.IsFailure)
	//		{
	//			return BadRequest(registerResponse);
	//		}
	//		return Ok(response);
	//	}
	//	catch (Exception exception)
	//	{
	//		return BadRequest(Result.Failure(new Error("Model State", exception.Message)));
	//	}
	//}


	[HttpGet("GetAllCustomers")]
	[HasPermission(AuthorizationPermissions.ReadSystemInformation)]
	public async Task<IActionResult> GetAllCustomers()
	{
		Result<List<Customer>> response = await _sender.Send(new GetAllCustomersQuery());

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(response.Value.Select(customer => _mapper.Map(customer)).ToList());
	}

	[HttpGet("GetPaginatedCustomers")]
	[HasPermission(AuthorizationPermissions.ReadSystemInformation)]
	public async Task<IActionResult> GetPaginatedCustomers(int pageNumber, int pageSize)
	{
		Result<List<Customer>> response = await _sender.Send(new GetCustomersPageQuery(pageSize, pageNumber));

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(response.Value.Select(customer => _mapper.Map(customer)).ToList());
	}

	[HttpGet("GetCustomerBySerialNumber")]
	[HasPermission(AuthorizationPermissions.ReadSystemInformation)]
	public async Task<IActionResult> GetCustomerBySerialNumber(int serialNumber)
	{
		Result<Customer> response = await _sender.Send(new GetCustomerBySerialNumberQuery(serialNumber));

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(_mapper.Map(response.Value));
	}

	[HttpGet("GetSumOfCustomersBalances")]
	[HasPermission(AuthorizationPermissions.ReadSystemInformation)]
	public async Task<IActionResult> GetSumOfCustomersBalances()
	{
		Result<long> response = await _sender.Send(new GetSumOfCustomersBalancesQuery());

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(response.Value);
	}

	[HttpPut("IncreaseCustomerBalance")]
	[HasPermission(AuthorizationPermissions.EditBalances)]
	public async Task<IActionResult> IncreaseCustomerBalance([FromForm] IncreaseBalanceRequest balanceRequest)
	{
		Result response = await _sender.Send(new IncreaseCustomerBalanceCommand(balanceRequest.serialNumber, balanceRequest.valueToAdd));

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(response);
	}

	[HttpPut("DecreaseCustomerBalance")]
	[HasPermission(AuthorizationPermissions.CreateSystemInformation)]
	public async Task<IActionResult> DecreaseCustomerBalance(int serialNumber, int valueToRemove)
	{
		Result response = await _sender.Send(new DecreaseCustomerBalanceCommand(serialNumber, valueToRemove));

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(response);
	}

	[HttpGet("AccountTransactionsPage/{pageNumber}")]
	[HasPermission(AuthorizationPermissions.ReadContent)]
	public async Task<IActionResult> GetAccountTransactionsPage(int pageNumber)
	{
		int serialNumber = int.Parse(_executorIdentityProvider.GetExecutorSerialNumber());
		Result<Domain.Customers.ReadModels.AccountTransactionsReadModel> response = await _sender.Send(
			new GetAccountTransactionsQuery(serialNumber, pageNumber));

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(response);
	}
}
