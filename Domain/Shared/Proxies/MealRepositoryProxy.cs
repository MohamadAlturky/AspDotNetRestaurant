using Domain.Meals.Aggregate;
using Domain.Meals.Repositories;

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
