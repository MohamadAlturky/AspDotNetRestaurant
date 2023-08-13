using Domain.Meals.Repositories;
using Domain.Meals.ValueObjects;
using SharedKernal.CQRS.Queries;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Result;

namespace Application.Meals.UseCases.GetAutoCompleteMealName;
internal class GetAutoCompleteMealNameQueryHandler : IQueryHandler<GetAutoCompleteMealNameQuery, List<AutoCompleteModel>>
{
	private readonly IUnitOfWork _unitOfWork;

	private readonly IMealRepository _mealRepository;

	public GetAutoCompleteMealNameQueryHandler(IUnitOfWork unitOfWork, IMealRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealRepository = mealRepository;
	}

	public async Task<Result<List<AutoCompleteModel>>> Handle(GetAutoCompleteMealNameQuery request, 
		CancellationToken cancellationToken)
	{
		try
		{
			List<AutoCompleteModel> response = _mealRepository.GetAutoComplete(request.partOfMealName, request.mealType);
			await _unitOfWork.SaveChangesAsync();
			return Result.Success(response);

		}
		catch (Exception exception)
		{
			return Result.Failure<List<AutoCompleteModel>>(new SharedKernal.Utilities.Errors.Error("", exception.Message));
		}
	}
}
