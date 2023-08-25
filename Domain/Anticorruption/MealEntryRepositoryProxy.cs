using Domain.MealEntries.Aggregate;
using Domain.Meals.Repositories;

namespace Domain.Shared.Proxies;
public class MealEntryRepositoryProxy
{
	private readonly IMealEntryRepository _mealRepository;

	public MealEntryRepositoryProxy(IMealEntryRepository mealRepository)
	{
		_mealRepository = mealRepository;
		
	}
	public MealEntry? GetMealEntry(long id)
	{
		return _mealRepository.GetMealEntryById(id);
	}
}
