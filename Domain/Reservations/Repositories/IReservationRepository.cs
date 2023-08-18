using Domain.Reservations.Aggregate;
using SharedKernal.Repositories;

namespace Domain.Reservations.Repositories;
public interface IReservationRepository : IRepository<Reservation>
{
	IEnumerable<Reservation> GetAll();
	IEnumerable<Reservation> GetPage(int pageSize, int pageNumber);
	Reservation? GetById(long id);


	void Add(Reservation Entity);
	void Update(Reservation Entity);
	void Delete(Reservation Entity);

	bool CheckIfCustomerHasAMealReservation(long customerId, long mealEntryId);
	List<Reservation> GetByDate(DateOnly day);
	List<Reservation> GetBetweenTwoDate(DateOnly start, DateOnly end);
	List<Reservation> GetByCustomer(long id);
	List<Reservation> GetOnWaitingToCancel(long mealEntryId);
	Reservation? GetFirstOnWaitingToCancelWhereHisBalanceIsEnough(long mealEntryId);
	List<Reservation> GetTheKFirstWaitingReservationsOnEntry(long mealEntryId,int K);
	Reservation? GetFirstWaitingReservationsOnEntry(long entryId);
	void DeleteAll(ICollection<Reservation> reservations);
	Reservation? GetReservationOnMealEntryBySerialNumber(long mealEntryId, int serialNumber);
}
