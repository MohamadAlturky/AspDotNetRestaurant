using Domain.Reservations.Aggregate;
using SharedKernal.Repositories;

namespace Domain.Reservations.Repositories;
public interface IReservationRepository : IRepository<Reservation>
{
	bool CheckIfCustomerHasAMealReservation(long customerId, long mealEntryId);
	List<Reservation> GetByDate(DateOnly day);
	List<Reservation> GetBetweenTwoDate(DateOnly start, DateOnly end);
	List<Reservation> GetByCustomer(long id);
	List<Reservation> GetOnWaitingToCancel(long mealEntryId);
	Reservation? GetFirstOnWaitingToCancel(long mealEntryId);
	List<Reservation> GetTheKFirstWaitingReservationsOnEntry(long mealEntryId,int K);
	Reservation? GetFirstWaitingReservationsOnEntry(long entryId);
}
