using Domain.Meals.Entities;
using Domain.Meals.Repositories;
using SharedKernal.CQRS.Commands;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;

namespace Application.Meals.UseCases.PrepareNewMeal;
internal class PrepareNewMealCommandHandler : ICommandHandler<PrepareNewMealCommand>
{
	private IUnitOfWork _unitOfWork { get; set; }

	private IMealRepository _mealRepository { get; set; }

	public PrepareNewMealCommandHandler(IUnitOfWork unitOfWork, IMealRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealRepository = mealRepository;
	}

	public async Task<Result> Handle(PrepareNewMealCommand request, CancellationToken cancellationToken)
	{
		try
		{
			MealInformation? meal = _mealRepository.GetInformationById(request.mealId);

			if(meal is null)
			{
				return Result.Failure(new Error("", "if(meal is null)"));
			}
			
			DateTime date = new DateTime(request.atDay.Year, 
				request.atDay.Month,
				request.atDay.Day);


			bool checkIfMealHasEntryInDay = _mealRepository.CheckIfMealHasEntryInDay(request.mealId, request.atDay);

			meal.PrepareNewEntry(date, request.numberOfUnits, checkIfMealHasEntryInDay);

			_mealRepository.UpdateInformation(meal);

			await _unitOfWork.SaveChangesAsync();
		}
		catch (Exception exception)
		{
			return Result.Failure(new Error("", exception.Message));
		}

		return Result.Success();
	}
}