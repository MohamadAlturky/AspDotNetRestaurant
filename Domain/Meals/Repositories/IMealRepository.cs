using SharedKernal.Repositories;
using Domain.Meals.Aggregate;
using Domain.Shared.Entities;
using Domain.Meals.ValueObjects;
using Domain.Shared.ReadModels;

namespace Domain.Meals.Repositories;

public interface IMealRepository : IRepository<Meal>
{
	IEnumerable<MealEntry> GetMealEntries(long mealId);
	public MealEntry? GetMealEntry(long entryId);
	List<MealEntry> GetEntriesByDate(DateOnly date);
	Meal? GetMealWithEntry(long id,Func<MealEntry,bool> entrySelector);
	bool CheckIfMealHasEntryInDay(long mealId, DateOnly day);
	List<AutoCompleteModel> GetAutoComplete(string partOfMealName, MealType mealType);
	List<Meal> GetEntriesByNameAndType(string mealName, MealType type);
	WeeklyPreparedMeals GetWeeklyMealsStartsFrom(DateTime startOfTheWeek,long customerId);
}
