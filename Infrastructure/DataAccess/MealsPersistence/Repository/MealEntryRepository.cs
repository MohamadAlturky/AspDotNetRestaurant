using Domain.MealEntries.Aggregate;
using Domain.MealInformations.Aggregate;
using Domain.Meals.Repositories;
using Domain.Meals.ValueObjects;
using Domain.Shared.ReadModels;
using Infrastructure.DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MealsPersistence.Repository;

public class MealEntryRepository : IMealEntryRepository
{
	private readonly RestaurantContext _context;

	public MealEntryRepository(RestaurantContext context)
	{
		_context = context;
	}

	public List<MealEntry> GetEntriesByDate(DateOnly date)
	{
		DateTime dateTime = new DateTime(date.Year, date.Month, date.Day);
		return _context.Set<MealEntry>()
			.Where(entry => entry.AtDay == dateTime)
			.Include(entry => entry.MealInformation)
			.AsNoTracking()
			.ToList();
	}


	public IEnumerable<MealEntry> GetMealEntries(long mealId)
	{
		return _context.Set<MealEntry>()
			.Where(mealEntry => mealEntry.MealInformationId == mealId)
			.AsNoTracking()
			.ToList();
	}

	public MealEntry? GetMealEntryById(long entryId)
	{
		return _context.Set<MealEntry>()
			.Include(entry => entry.MealInformation)
			.Where(entry => entry.Id == entryId).FirstOrDefault();
	}

	public List<AutoCompleteModel> GetAutoComplete(string partOfMealName, MealType mealType)
	{
		var type = mealType.ToString();
		return _context.Set<MealInformation>()
			.Where(meal => meal.Type == type)
			.Where(meal => meal.Name.Contains(partOfMealName))
			.Select(meal => new AutoCompleteModel(meal.Id, meal.Name))
			.ToList();
	}

	public List<MealInformation> GetEntriesByNameAndType(string mealName, MealType type)
	{
		var stringType = type.ToString();
		return _context.Set<MealInformation>()
			.Where(meal => meal.Type == stringType)
			.Where(meal => meal.Name.Contains(mealName))
			.ToList();
	}

	public WeeklyPreparedMeals GetWeeklyMealsStartsFrom(DateTime startOfTheWeek, long customerId)
	{
		DateTime endOfTheWeek = startOfTheWeek.AddDays(6);
		WeeklyPreparedMeals response = new WeeklyPreparedMeals();

		response.StartDay = new DateOnly(startOfTheWeek.Year,
										 startOfTheWeek.Month,
										 startOfTheWeek.Day);

		var meals = _context.Set<MealEntry>()
			.Where(entry => entry.AtDay >= startOfTheWeek)
			.Where(entry => entry.AtDay <= endOfTheWeek)
			.Include(entry => entry.MealInformation)
			.Include(entry => entry.Reservations
				.Where(res => res.CustomerId == customerId))
			.OrderBy(entry => entry.AtDay)
			.ToList();

		foreach (var entry in meals)
		{
			if (entry.MealInformation is null)
			{
				throw new Exception("if (entry.Meal == null)");
			}
			// error when try to parse to json
		}

		for (int index = 0; index <= 6; index++)
		{
			DailyMeals daily = new DailyMeals();
			daily.AtDay = new DateOnly(startOfTheWeek.AddDays(index).Year,
									   startOfTheWeek.AddDays(index).Month,
									   startOfTheWeek.AddDays(index).Day);

			var dailyMeals = meals.Where(entry =>
				entry.AtDay == startOfTheWeek.AddDays(index))
				.Select(entry => ReadModelsMapper.Map(entry)).ToList();

			foreach (var meal in dailyMeals)
			{
				daily.Meals.Add(meal);
			}

			response.Dailies.Add(daily);
		}

		return response;
	}

	public void Add(MealEntry newMeal)
	{
		newMeal.Id = 0;
		_context.Set<MealEntry>().Add(newMeal);
	}

	public MealEntry? GetMealEntryWithAllInformationAboutReservationsAndCustomers(long mealEntryId)
	{
		return _context.Set<MealEntry>()
			.Include(mealEntry => mealEntry.Reservations)
			.ThenInclude(reservatoin=>reservatoin.Customer)
			.FirstOrDefault(mealEntry => mealEntry.Id == mealEntryId);
	}

	public void Delete(MealEntry fullMealEntry)
	{
		_context.Set<MealEntry>().Remove(fullMealEntry);
	}
}
