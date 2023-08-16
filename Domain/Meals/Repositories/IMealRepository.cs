using SharedKernal.Repositories;
using Domain.Meals.ValueObjects;
using Domain.Shared.ReadModels;
using Domain.Meals.Entities;
using Domain.Meals.Aggregate;

namespace Domain.Meals.Repositories;

public interface IMealRepository : IRepository<MealEntry>
{
	IEnumerable<MealInformation> GetAllInformations();
	IEnumerable<MealInformation> GetInformationsPage(int pageSize, int pageNumber);
	MealInformation? GetInformationById(long id);


	void AddInformation(MealInformation Entity);
	void UpdateInformation(MealInformation Entity);
	void DeleteInformation(MealInformation Entity);
	 

	IEnumerable<MealEntry> GetMealEntries(long mealId);
	public MealEntry? GetMealEntry(long entryId);
	List<MealEntry> GetEntriesByDate(DateOnly date);
	MealInformation? GetMealWithEntry(long id,Func<MealEntry,bool> entrySelector);
	bool CheckIfMealHasEntryInDay(long mealId, DateOnly day);
	List<AutoCompleteModel> GetAutoComplete(string partOfMealName, MealType mealType);
	List<MealInformation> GetEntriesByNameAndType(string mealName, MealType type);
	WeeklyPreparedMeals GetWeeklyMealsStartsFrom(DateTime startOfTheWeek,long customerId);
}
