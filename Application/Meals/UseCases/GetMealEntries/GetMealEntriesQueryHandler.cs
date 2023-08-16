using Domain.Meals.Aggregate;
using Domain.Meals.Repositories;
using SharedKernal.CQRS.Queries;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Errors;
using SharedKernal.Utilities.Result;

namespace Application.Meals.UseCases.GetMealEntries;
internal class GetMealEntriesQueryHandler : IQueryHandler<GetMealEntriesQuery, List<MealEntry>>
{
	private readonly IUnitOfWork _unitOfWork;

	private readonly IMealRepository _mealRepository;

	public GetMealEntriesQueryHandler(IUnitOfWork unitOfWork, IMealRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealRepository = mealRepository;
	}

	public async Task<Result<List<MealEntry>>> Handle(GetMealEntriesQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var response =_mealRepository.GetMealEntries(request.mealId).ToList();
			await _unitOfWork.SaveChangesAsync();
			return Result.Success(response);
		}
		catch(Exception exception)
		{
			return Result.Failure<List<MealEntry>>(new Error("", exception.Message));

		}
	}
}
