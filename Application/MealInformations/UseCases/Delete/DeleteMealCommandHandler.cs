using Domain.MealInformations.Aggregate;
using Domain.MealInformations.Repositories;
using SharedKernal.CQRS.Commands;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Result;

namespace Application.Meals.UseCases.Delete;
internal class DeleteMealCommandHandler : ICommandHandler<DeleteMealCommand>
{
	private IUnitOfWork _unitOfWork { get; set; }

	private IMealInformationRepository _mealInformationRepository { get; set; }

	public DeleteMealCommandHandler(IUnitOfWork unitOfWork, IMealInformationRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealInformationRepository = mealRepository;
	}

	public async Task<Result> Handle(DeleteMealCommand request, CancellationToken cancellationToken)
	{
		try
		{
			_mealInformationRepository.DeleteInformation(new MealInformation(request.mealId));

			await _unitOfWork.SaveChangesAsync();
		}
		catch (Exception exception)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", exception.Message));
		}
		return Result.Success();
	}
}
