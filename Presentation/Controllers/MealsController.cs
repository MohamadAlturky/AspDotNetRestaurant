using Application.IdentityChecker;
using Application.MealEntries.UseCases.CancelMealEntry;
using Application.Meals.UseCases.GetMealEntries;
using Application.Meals.UseCases.GetMealEntriesByDate;
using Application.Meals.UseCases.GetMealsByName;
using Application.Meals.UseCases.GetMealsSchedule;
using Application.Meals.UseCases.GetWeeklyMeals;
using Application.Meals.UseCases.PrepareNewMeal;
using Application.UseCases.Meals.Create;
using Application.UseCases.Meals.GetAll;
using Domain.Meals.ValueObjects;
using Infrastructure.Authentication.Permissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.ApiModels;
using Presentation.ApiModels.MealEntry;
using Presentation.ApiModels.Meals;
using Presentation.Mappers;
using Presentation.PermissionsContainer;
using Presentation.Services.MealsImagesSaver;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;

namespace Presentation.Controllers;


[Route("api/[controller]")]
[ApiController]
public class MealsController : APIController
{
	private readonly ILogger<MealsController> _logger;
	private readonly IAssetsSaver _assetsSaver;
	private readonly IExecutorIdentityProvider _identityProvider;
	public MealsController(ISender sender,
		IMapper mapper,
		ILogger<MealsController> logger,
		IAssetsSaver assetsSaver,
		IExecutorIdentityProvider identityProvider
		)
		: base(sender, mapper)
	{
		_assetsSaver = assetsSaver;
		_logger = logger;
		_identityProvider = identityProvider;
	}


	[HttpPost("Create")]
	[HasPermission(AuthorizationPermissions.CreateSystemInformation)]
	public async Task<IActionResult> Create([FromForm] CreateMealRequest meal)
	{
		try
		{
			MealDTO mealDTO = new MealDTO()
			{
				Id = 0,
				ImagePath="",
				Type=meal.Type,
				Name = meal.Name,
				Description = meal.Description,
				NumberOfCalories=meal.NumberOfCalories
			};
			IFormFile file = meal.ImageFile;
			if (!ModelState.IsValid || !file.ContentType.StartsWith("image/"))
			{
				return BadRequest(Result.Failure(new Error("Model State", "Model State is not valid")));
			}
			var imagePathResult = await _assetsSaver.SaveFile(file, "MealsImages");
			if (imagePathResult.IsSuccess)
			{
				mealDTO.ImagePath = imagePathResult.Value;
			}
			else
			{
				throw new Exception(imagePathResult.Error.Message);
			}
			Result response = await _sender.Send(new CreateMealCommand(_mapper.Map(mealDTO)));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(response.Error));
			}

			return Ok(response);
		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error(exception.StackTrace.ToString(), exception.Message)));
		}
	}

	//[HttpPost("CreateFile")]
	//public async Task<IActionResult> CreateFile(IFormFile ImageFile)
	//{
	//	return Ok("ImageFile");
	//}

	[HttpDelete("DeleteMealEntry")]
	[HasPermission(AuthorizationPermissions.CreateSystemInformation)]
	public async Task<IActionResult> DeleteMealEntry(long mealEntryId)
	{
		var response = await _sender.Send(new CancelMealEntryCommand(mealEntryId));

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(Result.Success());
	}


	[HttpGet("GetAllMeals")]
	[HasPermission(AuthorizationPermissions.ReadSystemInformation)]
	public async Task<IActionResult> GetAllMeals()
	{
		var response = await _sender.Send(new GetMealsQuery());

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(Result.Success(response.Value.Select(meal => _mapper.Map(meal)).ToList()));
	}

	[HttpGet("GetMealsByNameAndType/{mealName}/{mealType}")]
	[HasPermission(AuthorizationPermissions.ReadSystemInformation)]
	public async Task<IActionResult> GetMealsByNameAndType(string mealName,string mealType)
	{
		MealType type = Enum.Parse<MealType>(mealType);
		var response = await _sender.Send(new GetMealsByNameQuery(mealName, type));

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(Result.Success(response.Value.Select(meal => _mapper.Map(meal)).ToList()));
	}

	[HttpGet("GetMealEntries")]
	[HasPermission(AuthorizationPermissions.ReadSystemInformation)]
	public async Task<IActionResult> GetMealEntries(long Id)
	{
		var response = await _sender.Send(new GetMealEntriesQuery(Id));

		if (response.IsFailure)
		{
			return BadRequest(Result.Failure(response.Error));
		}

		return Ok(response.Value);
	}

	[HttpGet("GetMealEntriesByDate")]
	[HasPermission(AuthorizationPermissions.ReadSystemInformation)]
	public async Task<IActionResult> GetMealEntriesByDate(string date)
	{

		try
		{
			DateOnly dateFilter = DateOnly.Parse(date);

			var response = await _sender.Send(new GetMealEntriesByDateQuery(dateFilter));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(response.Error));
			}
			return Ok(Result.Success(response.Value.Select(entry => _mapper.Map(entry))));

		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("", exception.Message)));
		}
	}


	[HttpGet("GetMealEntriesByDateForConsume")]
	[HasPermission(AuthorizationPermissions.ConsumeReservations)]
	public async Task<IActionResult> GetMealEntriesByDateForConsume(string date)
	{

		try
		{
			DateOnly dateFilter = DateOnly.Parse(date);

			var response = await _sender.Send(new GetMealEntriesByDateQuery(dateFilter));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(response.Error));
			}
			return Ok(Result.Success(response.Value.Select(entry => _mapper.Map(entry))));

		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("", exception.Message)));
		}
	}


	[HttpGet("GetWeeklyMeals")]
	[HasPermission(AuthorizationPermissions.ReadContent)]
	public async Task<IActionResult> GetWeeklyMeals(int WeekNumber)
	{
		try
		{
			DateTime target = DateTime.Now.AddDays(7 * WeekNumber);
			
			DateOnly dateFilter = new DateOnly(target.Year,target.Month,target.Day);

			long id = long.Parse(_identityProvider.GetExecutorId());

			var response = await _sender.Send(new GetWeeklyMealsQuery(dateFilter,id));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(response.Error));
			}
			return Ok(Result.Success(response.Value));

		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("", exception.Message)));
		}
	}

	[HttpGet("GetMealsSchedule")]
	public async Task<IActionResult> GetMealsSchedule(int WeekNumber)
	{
		try
		{
			DateTime target = DateTime.Now.AddDays(7 * WeekNumber);

			DateOnly dateFilter = new DateOnly(target.Year, target.Month, target.Day);

			var response = await _sender.Send(new GetMealsScheduleQuery(dateFilter));

			if (response.IsFailure)
			{
				return BadRequest(Result.Failure(response.Error));
			}
			return Ok(Result.Success(response.Value));

		}
		catch (Exception exception)
		{
			return BadRequest(Result.Failure(new Error("", exception.Message)));
		}
	}


	[HttpPost("PrepareMeal")]
	[HasPermission(AuthorizationPermissions.CreateSystemInformation)]
	public async Task<IActionResult> PrepareMeal([FromForm] PrepareRequest request)
	{
		try
		{
			DateOnly atDayValue = DateOnly.Parse(request.atDay);
			Result response = await _sender.Send(new PrepareNewMealCommand(request.mealId, atDayValue, request.numberOfUnits));

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

	
}
