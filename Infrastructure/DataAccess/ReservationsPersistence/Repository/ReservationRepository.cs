using Domain.Customers.Aggregate;
using Domain.MealEntries.Aggregate;
using Domain.Reservations.Aggregate;
using Domain.Reservations.Repositories;
using Domain.Reservations.ValueObjects;
using Infrastructure.DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ReservationsPersistence.Repository;
public class ReservationRepository : IReservationRepository
{


	private readonly RestaurantContext _context;
	public ReservationRepository(RestaurantContext context)
	{
		_context = context;
	}



	public void Add(Reservation Entity)
	{
		Entity.Id = 0;
		_context.Set<Reservation>().Add(Entity);
	}

	public bool CheckIfCustomerHasAMealReservation(long customerId, long mealEntryId)
	{
		return _context.Set<Reservation>()
			   .Where(reservation => reservation.CustomerId == customerId)
			   .Where(reservation => reservation.MealEntryId == mealEntryId)
			   .Any();
	}

	public void Delete(Reservation Entity)
	{
		throw new NotImplementedException();
		
	}

	public void DeleteAll(ICollection<Reservation> reservations)
	{
		_context.Set<Reservation>().RemoveRange(reservations);
	}

	public IEnumerable<Reservation> GetAll()
	{
		throw new NotImplementedException();
	}

	public List<Reservation> GetBetweenTwoDate(DateOnly start, DateOnly end)
	{
		DateTime normalizedStart = new DateTime(start.Year, start.Month, start.Day);
		DateTime normalizedEnd = new DateTime(end.Year, end.Month, end.Day);

		return _context.Set<Reservation>()
			.Where(reservation => reservation.AtDay >= normalizedStart && reservation.AtDay <= normalizedEnd)
			.ToList();
	}

	public List<Reservation> GetByCustomer(long id)
	{
		return _context.Set<Reservation>()
			.Where(reservation => reservation.CustomerId == id)
			.ToList();
	}

	public List<Reservation> GetByDate(DateOnly day)
	{
		DateTime normalizedDay = new DateTime(day.Year, day.Month, day.Day);

		return _context.Set<Reservation>()
			.Where(reservation => reservation.AtDay == normalizedDay)
			.ToList();
	}

	public Reservation? GetById(long id)
	{
		return _context.Set<Reservation>().Find(id);
	}

	public Reservation? GetFirstOnWaitingToCancelWhereHisBalanceIsEnough(long mealEntryId)
	{
		return _context.Set<Reservation>()
		.Where(reservation => reservation.MealEntryId == mealEntryId)
		.Where(reservation => reservation.ReservationStatus ==
				OrderStatus.OnTheCanceledListButNotCanceledYet.ToString())
		.Include(reservation => reservation.Customer)
		.Where(reservation => reservation.Customer.Balance > reservation.Price)
		.OrderBy(reservation => reservation.NumberInQueue)
		.FirstOrDefault();
	}

	public Reservation? GetFirstWaitingReservationsOnEntry(long entryId)
	{
		return _context.Set<Reservation>()
							.Where(reservation => reservation.MealEntryId == entryId)
							.Where(reservation => reservation.ReservationStatus == OrderStatus.Waiting.ToString())
							.Include(reservation => reservation.Customer)
							.OrderBy(reservation => reservation.NumberInQueue)
							.FirstOrDefault();
	}


	public List<Reservation> GetOnWaitingToCancel(long mealEntryId)
	{
		return _context.Set<Reservation>()
		.Where(reservation => reservation.MealEntryId == mealEntryId)
		.Where(reservation => reservation.ReservationStatus == 
				OrderStatus.OnTheCanceledListButNotCanceledYet.ToString())
		.Include(reservation => reservation.Customer)
		.OrderBy(reservation => reservation.NumberInQueue)
		.ToList();
	}

	public IEnumerable<Reservation> GetPage(int pageSize, int pageNumber)
	{
		throw new NotImplementedException();
	}

	public Reservation? GetReservationOnMealEntryBySerialNumber(long mealEntryId, int serialNumber)
	{
		return _context.Set<Reservation>()
			.Where(reservation => reservation.MealEntryId == mealEntryId)
			.Include(reservation => reservation.Customer)
			.Where(reservation => reservation.Customer.SerialNumber == serialNumber)
			.FirstOrDefault();
	}

	//public Reservation? GetTheFirstOfWaitingReservationsOnEntry(long id)
	//{
	//	return _context.Set<Reservation>()
	//		.Where(reservation=> reservation.MealEntryId == id)
	//		.Where(reservation => reservation.ReservationStatus == OrderStatus.Waiting.ToString())
	//		.OrderBy(reservation => reservation.NumberInQueue)
	//		.FirstOrDefault();
	//}

	public List<Reservation> GetTheKFirstWaitingReservationsOnEntry(long mealEntryId, int K)
	{
		return _context.Set<Reservation>()
		.Where(reservation => reservation.MealEntryId == mealEntryId)
		.Where(reservation => reservation.ReservationStatus == OrderStatus.Waiting.ToString())
		.Include(reservation=> reservation.Customer)
		.OrderBy(reservation => reservation.NumberInQueue)
		.Take(K)
		.ToList();
	}

	public void Update(Reservation Entity)
	{
		_context.Set<Reservation>().Update(Entity);
	}
}
