using Domain.MealEntries.Aggregate;
using Domain.MealInformations.Aggregate;
using Domain.MealInformations.Repositories;
using Infrastructure.DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.MealsInformationPersistance;
public class MealInformationRepository : IMealInformationRepository
{
	private readonly RestaurantContext _context;

	public MealInformationRepository(RestaurantContext context)
	{
		_context = context;
	}
	public MealInformation? GetMealWithEntry(long id, Func<MealEntry, bool> entrySelector)
	{
		return _context.Set<MealInformation>()
			   .Where(meal => meal.Id == id)
			   .Include(meal => meal.MealEntries
				   .Where(entry => entrySelector(entry)))
			   .FirstOrDefault();
	}
	public bool CheckIfMealHasEntryInDay(long mealId, DateOnly day)
	{
		DateTime normalizedDay = new DateTime(day.Year, day.Month, day.Day);
		return _context.Set<MealEntry>()
		   .Where(entry => entry.MealInformationId == mealId)
		   .Where(entry => entry.AtDay == normalizedDay)
		   .Any();
	}
	public void AddInformation(MealInformation Entity)
	{
		// we will make the id = 0 to let EF Core set the primary key with auto increament value
		Entity.Id = 0;

		_context.Set<MealInformation>().Add(Entity);
	}

	public IEnumerable<MealInformation> GetAllInformations()
	{
		return _context.Set<MealInformation>().ToList();
	}


	public void DeleteInformation(MealInformation Entity)
	{
		throw new NotImplementedException();
	}

	public MealInformation? GetInformationById(long id)
	{
		return _context.Set<MealInformation>().Find(id);
	}
	public IEnumerable<MealInformation> GetInformationsPage(int pageSize, int pageNumber)
	{
		throw new NotImplementedException();
	}

	public void UpdateInformation(MealInformation Entity)
	{
		_context.Set<MealInformation>().Update(Entity);
	}

	public int GetNumberOfRecordsForPaginiation()
	{
		throw new NotImplementedException();
	}
}
