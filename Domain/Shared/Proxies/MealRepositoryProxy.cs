using Domain.Meals.Repositories;
using Domain.Shared.Entities;

namespace Domain.Shared.Proxies;
public class MealRepositoryProxy
{
	private readonly IMealRepository _mealRepository;

	public MealRepositoryProxy(IMealRepository mealRepository)
	{
		_mealRepository = mealRepository;
		
	}
	public MealEntry? GetMealEntry(long id)
	{
		return _mealRepository.GetMealEntry(id);
	}
}
