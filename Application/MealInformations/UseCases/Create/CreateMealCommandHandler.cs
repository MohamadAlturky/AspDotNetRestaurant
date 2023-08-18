using Domain.MealInformations.Repositories;
using SharedKernal.CQRS.Commands;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Result;

namespace Application.UseCases.Meals.Create;
public class GetMealsCommandHandler : ICommandHandler<CreateMealCommand>
{
	private IUnitOfWork _unitOfWork { get; set; }

	private IMealInformationRepository _mealRepository { get; set; }


	public GetMealsCommandHandler(IUnitOfWork unitOfWork, IMealInformationRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealRepository = mealRepository;
	}


	public async Task<Result> Handle(CreateMealCommand request, CancellationToken cancellationToken)
	{

		try
		{
			_mealRepository.AddInformation(request.Meal);

			await _unitOfWork.SaveChangesAsync();
		}
		catch(Exception exception)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("",exception.Message));
		}

		return Result.Success();
	}
}

