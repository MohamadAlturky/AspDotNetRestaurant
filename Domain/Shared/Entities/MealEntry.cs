using Domain.Meals.Aggregate;
using Domain.Meals.ValueObjects;
using Domain.Reservations.Aggregate;
using Domain.Shared.ValueObjects;
using SharedKernal.Entities;

namespace Domain.Shared.Entities;
public class MealEntry : Entity
{

	#region Private Data Members
	private NumberOfPreparedMeals _preparedCount = new NumberOfPreparedMeals(0);
	private NumberOfReservations _reservationsCount = new NumberOfReservations(0);
	private LastNumberInQueue _lastNumberInQueue = new LastNumberInQueue(0);
	#endregion

	#region Properties
	public long MealId { get; set; }
	public bool CustomerCanCancel { get; set; } = true;
	public DateTime AtDay { get; set; } = new ();
	public int PreparedCount { get => _preparedCount.Value; set => _preparedCount = new NumberOfPreparedMeals(value); }
	public int LastNumberInQueue { get => _lastNumberInQueue.Value; set => _lastNumberInQueue = new LastNumberInQueue(value); }
	public int ReservationsCount { get => _reservationsCount.Value; set => _reservationsCount = new NumberOfReservations(value); }
	public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
	#endregion

	#region Navidations
	public Meal? Meal { get; set; }
	#endregion

	#region Constructors
	public MealEntry(long id, long mealId) : base(id) { MealId = mealId; }
	public MealEntry() : base(0) { MealId = 0; }
	#endregion

	#region Functionality
	public void ModifyCancellationState(bool state)
	{
		CustomerCanCancel = state;
	}

	public void AddNewReservation()
	{
		ReservationsCount++;
		LastNumberInQueue++;
	}

	public void CancelReservation()
	{
		ReservationsCount--;
	}


	#endregion
}
