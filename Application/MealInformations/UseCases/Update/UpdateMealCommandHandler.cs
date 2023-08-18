using Domain.MealInformations.Repositories;
using Domain.Meals.Repositories;
using SharedKernal.CQRS.Commands;
using SharedKernal.Repositories;
using SharedKernal.Utilities.Result;

namespace Application.Meals.UseCases.Update;
internal class UpdateMealCommandHandler : ICommandHandler<UpdateMealCommand>
{
	private readonly IUnitOfWork _unitOfWork;

	private readonly IMealInformationRepository _mealInformationRepository;

	public UpdateMealCommandHandler(IUnitOfWork unitOfWork, 
		IMealInformationRepository mealRepository)
	{
		_unitOfWork = unitOfWork;
		_mealInformationRepository = mealRepository;
	}

	public async Task<Result> Handle(UpdateMealCommand request, 
		CancellationToken cancellationToken)
	{
		_mealInformationRepository.UpdateInformation(request.meal);
	
		await _unitOfWork.SaveChangesAsync();

		return Result.Success();
	}
}
