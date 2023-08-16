using Domain.Meals.Entities;
using Domain.Meals.Repositories;
using SharedKernal.CQRS.Queries;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;

namespace Application.UseCases.Meals.GetAll;
public class GetMealsQueryHandler : IQueryHandler<GetMealsQuery, List<MealInformation>>
{
	private readonly IUnitOfWork _unitOfWork;

	private readonly IMealRepository _mealRepository;

	public GetMealsQueryHandler(IUnitOfWork unitOfWork, IMealRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealRepository = mealRepository;
	}

	public async Task<Result<List<MealInformation>>> Handle(GetMealsQuery request, CancellationToken cancellationToken)
	{

		try
		{
			List<MealInformation> meals = _mealRepository.GetAllInformations().ToList();


			if (meals == null)
			{
				return Result.Failure<List<MealInformation>>(new Error("no data found", ""));
			}

			await _unitOfWork.SaveChangesAsync();

			return Result.Success(meals);
		}
		catch(Exception exception)
		{
			return Result.Failure<List<MealInformation>>(new Error("sorry", exception.Message));
		}
	}
}

