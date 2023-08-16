using Domain.Customers.Aggregate;
using Domain.Meals.Aggregate;
using Domain.Reservations.ValueObjects;
using Domain.Shared.Entities;
using Domain.Shared.Utilities;
using SharedKernal.Entities;

namespace Domain.Reservations.Aggregate;
public class Reservation : AggregateRoot
{
	private OrderStatus _status = OrderStatus.OrderCompletedButNotRegisteredYet;
	private NumberInQueue _numberInQueue = new NumberInQueue(0);
	private Price _price = new Price(0);


	public DateTime AtDay { get; set; } = new();
	public long CustomerId { get; set; } = 0;
	public long MealEntryId { get; set; } = 0;




	public string ReservationStatus { get => _status.ToString(); set => _status = Enum.Parse<OrderStatus>(value); }
	public int NumberInQueue { get => _numberInQueue.Value; set => _numberInQueue = new NumberInQueue(value); }
	public int Price { get => _price.Value; set => _price = new Price(value); }



	public Customer? Customer { get; set; }
	public MealEntry? MealEntry { get; set; }


	// constructor
	public Reservation(long id, long customerId, long orderedMealId) : base(id)
	{
		CustomerId = customerId;
		MealEntryId = orderedMealId;
	}
	public Reservation() : base(0) { }

	

	public void Cancel(MealEntry entry, Customer customer)
	{
		MealEntry = entry;

		Customer = customer;

		CheckCancellationValidity();

		if (Date.ToDay == entry.AtDay)
		{
			ReservationStatus = OrderStatus.OnTheCanceledListButNotCanceledYet.ToString();
			 
			//Raise(new SomeCustomerWantsToCancelHisReservationDomainEvent(entry.Id));
		}
		else if (Date.ToDay < entry.AtDay)
		{
			ReservationStatus = OrderStatus.Canceled.ToString();

			customer.IncreaseBalance(Price);

			entry.ReservationsCount--;

			//Raise(new SomeCustomerCanceledHisReservationDomainEvent(entry.Id));
		}
	}

	private void CheckCancellationValidity()
	{
		if (MealEntry is null)
		{
			throw new Exception("if(MealEntry is null)");
		}
		if (ReservationStatus == OrderStatus.Canceled.ToString())
		{
			throw new Exception("if (ReservationStatus == OrderStatus.Canceled.ToString())");
		}
		if (!MealEntry.CustomerCanCancel)
		{
			throw new Exception("!MealEntry.CustomerCanCancel");
		}

		if (Date.ToDay > MealEntry.AtDay)
		{
			throw new Exception("if(today > entry.AtDay)");
		}

		if (this.ReservationStatus == OrderStatus.Consumed.ToString())
		{
			throw new Exception("if(this.ReservationStatus == OrderStatus.مستهلكة.ToString())");
		}
	}

	internal void AcceptOnPromise()
	{
		ReservationStatus = OrderStatus.Reserved.ToString();
		Customer?.DecreaseBalance(Price);
	}

	internal void CancelOnPromise()
	{
		ReservationStatus = OrderStatus.Canceled.ToString();
		Customer?.IncreaseBalance(Price);
	}

	public static (Reservation, Reservation) Exchange(MealEntry entry,
		Customer customer,
		PricingRecord pricingRecord,
		long customerId,
		long orderedMealId,
		bool isCustomerHasAReservationOnThisEntry,
		Reservation firstReservationOnWaitingToCancel)
	{
		if (isCustomerHasAReservationOnThisEntry)
		{
			throw new Exception("CustomerHasAReservationOnThisEntry");
		}

		Reservation reservation = new Reservation(0, customerId, orderedMealId);

		reservation.MealEntry = entry;

		reservation.Customer = customer;

		if (customer.Balance < pricingRecord.Price)
		{
			throw new Exception("you Don't have money hahahahahaha");
		}

		if (entry.AtDay < Date.ToDay)
		{
			throw new Exception("entry.AtDay < Date.ToDay");
		}

		entry.LastNumberInQueue++;

		reservation.NumberInQueue = entry.LastNumberInQueue;
		reservation.AtDay = entry.AtDay;
		reservation.Price = pricingRecord.Price;


		customer.DecreaseBalance(pricingRecord.Price);
		reservation.ReservationStatus = OrderStatus.Reserved.ToString();

		firstReservationOnWaitingToCancel.
			Customer?
			.IncreaseBalance(firstReservationOnWaitingToCancel.Price);

		firstReservationOnWaitingToCancel.ReservationStatus = OrderStatus.Canceled.ToString();

		return (reservation, firstReservationOnWaitingToCancel);
	}



	public void CancelAndGiveMealTo(MealEntry entry, Customer customer, Reservation firstWaitingReservation)
	{
		MealEntry = entry;

		Customer = customer;

		CheckCancellationValidity();

		ReservationStatus = OrderStatus.Canceled.ToString();

		customer.IncreaseBalance(Price);

		if (firstWaitingReservation.Customer is null)
		{
			throw new Exception("if (firstWaitingReservation.Customer is null)");
		}

		firstWaitingReservation.Customer.DecreaseBalance(firstWaitingReservation.Price);

		firstWaitingReservation.ReservationStatus = OrderStatus.Reserved.ToString();
	}
}
