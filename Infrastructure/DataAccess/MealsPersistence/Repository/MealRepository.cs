using Domain.Meals.Aggregate;
using Domain.Meals.Entities;
using Domain.Meals.Repositories;
using Domain.Meals.ValueObjects;
using Domain.Shared.ReadModels;
using Infrastructure.DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MealsPersistence.Repository;

public class MealRepository : IMealRepository
{
	private readonly RestaurantContext _context;

	public MealRepository(RestaurantContext context)
	{
		_context = context;
	}


	public void AddInformation(MealInformation Entity)
	{
		// we will make the id = 0 to let EF Core set the primary key with auto increament value
		Entity.Id = 0;

		_context.Set<MealInformation>().Add(Entity);
	}

	public void DeleteInformation(MealInformation Entity)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<MealInformation> GetAllInformations()
	{
		return _context.Set<MealInformation>().ToList();
	}

	public List<MealEntry> GetEntriesByDate(DateOnly date)
	{
		DateTime dateTime = new DateTime(date.Year, date.Month, date.Day);
		return _context.Set<MealEntry>()
			.Where(entry => entry.AtDay == dateTime)
			.Include(entry => entry.Meal)
			.AsNoTracking()
			.ToList();
	}

	public MealInformation? GetInformationById(long id)
	{
		return _context.Set<MealInformation>().Find(id);
	}

	public IEnumerable<MealEntry> GetMealEntries(long mealId)
	{
		return _context.Set<MealEntry>()
			.Where(mealEntry => mealEntry.MealId == mealId)
			.AsNoTracking()
			.ToList();
	}

	public IEnumerable<MealInformation> GetInformationsPage(int pageSize, int pageNumber)
	{
		throw new NotImplementedException();
	}


	public void UpdateInformation(MealInformation Entity)
	{
		_context.Set<MealInformation>().Update(Entity);
	}

	public MealInformation? GetMealWithEntry(long id, Func<MealEntry, bool> entrySelector)
	{
		return _context.Set<MealInformation>()
			   .Where(meal => meal.Id == id)
			   .Include(meal => meal.MealEntries
				   .Where(entry => entrySelector(entry)))
			   .FirstOrDefault();
	}

	public MealEntry? GetMealEntry(long entryId)
	{
		return _context.Set<MealEntry>()
			.Include(entry=> entry.Meal)
			.Where(entry=> entry.Id==entryId).FirstOrDefault();
	}

	public bool CheckIfMealHasEntryInDay(long mealId, DateOnly day)
	{
		DateTime normalizedDay = new DateTime(day.Year,day.Month,day.Day);
		return _context.Set<MealEntry>()
		   .Where(entry => entry.MealId == mealId)
		   .Where(entry => entry.AtDay == normalizedDay)
		   .Any();
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

	public WeeklyPreparedMeals GetWeeklyMealsStartsFrom(DateTime startOfTheWeek,long customerId)
	{
		DateTime endOfTheWeek = startOfTheWeek.AddDays(6);
		WeeklyPreparedMeals response = new WeeklyPreparedMeals();
		
		response.StartDay = new DateOnly(startOfTheWeek.Year,
			    						 startOfTheWeek.Month,
									     startOfTheWeek.Day);

		var meals = _context.Set<MealEntry>()
			.Where(entry => entry.AtDay >= startOfTheWeek)
			.Where(entry => entry.AtDay <= endOfTheWeek)
			.Include(entry => entry.Meal)
			.Include(entry=>entry.Reservations
				.Where(res=>res.CustomerId== customerId))
			.OrderBy(entry=>entry.AtDay)
			.ToList();

		foreach(var entry in meals)
		{
			if (entry.Meal is null)
			{
				throw new Exception("if (entry.Meal == null)");
			}
			// error when try to parse to json
		}

		for(int index = 0; index <= 6; index++)
		{
			DailyMeals daily = new DailyMeals();
			daily.AtDay = new DateOnly(startOfTheWeek.AddDays(index).Year, 
			    					   startOfTheWeek.AddDays(index).Month, 
									   startOfTheWeek.AddDays(index).Day);

			var dailyMeals = meals.Where(entry =>
				entry.AtDay == startOfTheWeek.AddDays(index))
				.Select(entry => ReadModelsMapper.Map(entry)).ToList();

			foreach(var meal in dailyMeals)
			{
				daily.Meals.Add(meal);
			}

			response.Dailies.Add(daily);
		}

		return response;
	}
}
