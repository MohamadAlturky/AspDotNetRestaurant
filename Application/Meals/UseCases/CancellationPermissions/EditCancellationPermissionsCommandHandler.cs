using Application.Meals.UseCases.CancellationPermissions;
using Domain.Meals.Entities;
using Domain.Meals.Repositories;
using Domain.Shared.Entities;
using SharedKernal.CQRS.Commands;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;

namespace Application.Meals.UseCases.DisableAllowCancellation;
internal class EditCancellationPermissionsCommandHandler 
	: ICommandHandler<EditCancellationPermissionsCommand>
{
	private IUnitOfWork _unitOfWork { get; set; }

	private IMealRepository _mealRepository { get; set; }

	public EditCancellationPermissionsCommandHandler(
		IUnitOfWork unitOfWork, IMealRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealRepository = mealRepository;
	}

	public async Task<Result> Handle(EditCancellationPermissionsCommand request,
		CancellationToken cancellationToken)
	{
		try
		{
			MealInformation? meal = _mealRepository.GetMealWithEntry(request.mealId,
			entry => entry.Id == request.preparedMealId);


			if(meal is null)
			{
				#warning ddd
				throw new ArgumentException("");
			}


			meal.MealEntries.
				Where(entry => entry.Id == request.preparedMealId)
				.First().ModifyCancellationState(request.cancellationState);

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
