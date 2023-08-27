using Domain.Customers.Aggregate;
using Domain.MealEntries.Aggregate;
using Domain.Pricing.Aggregate;
using Domain.Reservations.Services;
using Xunit;

namespace UnitTests.DomainLayerTests;
public class CreateReservationTests
{

	[Fact] 
	public void CreateMethodOnReservationsService_ShouldReturn_ANewReservationWithStatusReserved()
	{
		// Arrange
		IReservationsService service = new ReservationsService();
		var meal = new MealEntry()
		{
			AtDay = DateTime.Now
			//,ConsumedReservations
		};
		var customer = new Customer();
		var record = new PricingRecord();
		var isHaveAReservation = true;
		service.Create(meal, customer, record, 1, 1, isHaveAReservation);

		// Act




		// Assert
	}
}
