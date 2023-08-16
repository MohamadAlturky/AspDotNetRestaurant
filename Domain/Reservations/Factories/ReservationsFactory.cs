using Domain.Customers.Aggregate;
using Domain.Meals.Aggregate;
using Domain.Reservations.Aggregate;
using Domain.Reservations.ValueObjects;
using Domain.Shared.Entities;
using Domain.Shared.Utilities;

namespace Domain.Reservations.Factories;
public static class ReservationsFactory
{
	public static Reservation Create(MealEntry entry, Customer customer, PricingRecord pricingRecord,
		long customerId, long orderedMealId, bool isCustomerHasAReservationOnThisEntry)
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


		if (entry.ReservationsCount > entry.PreparedCount)
		{
			reservation.ReservationStatus = OrderStatus.Waiting.ToString();
			//reservation.Raise(new SomeCustomerNewInTheWaitingListDomainEvent(entry.Id));
		}
		else
		{
			entry.ReservationsCount++;
			customer.DecreaseBalance(pricingRecord.Price);
			reservation.ReservationStatus = OrderStatus.Reserved.ToString();
		}

		return reservation;
	}
}
