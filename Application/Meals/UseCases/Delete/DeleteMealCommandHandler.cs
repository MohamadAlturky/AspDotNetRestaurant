using Domain.Meals.Repositories;
using SharedKernal.CQRS.Commands;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Result;
using Domain.Meals.Aggregate;
using Domain.Meals.Entities;

namespace Application.Meals.UseCases.Delete;
internal class DeleteMealCommandHandler : ICommandHandler<DeleteMealCommand>
{
	private IUnitOfWork _unitOfWork { get; set; }

	private IMealRepository _mealRepository { get; set; }

	public DeleteMealCommandHandler(IUnitOfWork unitOfWork, IMealRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealRepository = mealRepository;
	}

	public async Task<Result> Handle(DeleteMealCommand request, CancellationToken cancellationToken)
	{
		try
		{
			_mealRepository.DeleteInformation(new MealInformation(request.mealId));

			await _unitOfWork.SaveChangesAsync();
		}
		catch (Exception exception)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", exception.Message));
		}
		return Result.Success();
	}
}
