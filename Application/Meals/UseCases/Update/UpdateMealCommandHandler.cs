using Domain.Meals.Repositories;
using SharedKernal.CQRS.Commands;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Result;

namespace Application.Meals.UseCases.Update;
internal class UpdateMealCommandHandler : ICommandHandler<UpdateMealCommand>
{
	private IUnitOfWork _unitOfWork { get; set; }

	private IMealRepository _mealRepository { get; set; }

	public UpdateMealCommandHandler(IUnitOfWork unitOfWork, IMealRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealRepository = mealRepository;
	}

	public async Task<Result> Handle(UpdateMealCommand request, CancellationToken cancellationToken)
	{
		_mealRepository.Update(request.meal);
	
		await _unitOfWork.SaveChangesAsync();

		return Result.Success();
	}
}
