using Domain.Meals.Aggregate;
using Domain.Meals.Repositories;
using Domain.Shared.Entities;
using SharedKernal.CQRS.Queries;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Result;

namespace Application.Meals.UseCases.GetMealsByName;
internal class GetMealsByNameQueryHandler : IQueryHandler<GetMealsByNameQuery, List<Meal>>
{
	private readonly IUnitOfWork _unitOfWork;

	private readonly IMealRepository _mealRepository;

	public GetMealsByNameQueryHandler(IUnitOfWork unitOfWork, IMealRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealRepository = mealRepository;
	}

	public async Task<Result<List<Meal>>> Handle(GetMealsByNameQuery request, CancellationToken cancellationToken)
	{
		try
		{
			List<Meal> response = _mealRepository.GetEntriesByNameAndType(request.mealName, request.type);
			await _unitOfWork.SaveChangesAsync();
			return Result.Success(response);

		}
		catch (Exception exception)
		{
			return Result.Failure<List<Meal>>(new SharedKernal.Utilities.Errors.Error("", exception.Message));
		}
	}
}
