using Domain.Shared.Entities;

namespace Domain.Shared.ReadModels;
public static class ReadModelsMapper
{
	public static MealReadModel Map(MealEntry mealEntry)
	{
		if(mealEntry.Meal is null)
		{
			throw new Exception("if(mealEntry.Meal is null)");
		}

		string reservationStatus = "Empty";
		long reservationIdValue = 0;
		var reservation = mealEntry.Reservations.SingleOrDefault();

		if (reservation is not null)
		{
			reservationIdValue = reservation.Id;
			reservationStatus = reservation.ReservationStatus;
		}
		return new MealReadModel()
		{
			Id = mealEntry.Id,
			AtDay = mealEntry.AtDay,
			CustomerCanCancel = mealEntry.CustomerCanCancel,
			LastNumberInQueue = mealEntry.LastNumberInQueue,
			MealId = mealEntry.MealId,
			PreparedCount = mealEntry.PreparedCount,
			ReservationsCount = mealEntry.ReservationsCount,
			Type = mealEntry.Meal.Type,
			NumberOfCalories = mealEntry.Meal.NumberOfCalories,
			Name = mealEntry.Meal.Name,
			ImagePath = mealEntry.Meal.ImagePath,
			Description = mealEntry.Meal.Description,
			ReservationStatus = reservationStatus,
			reservationId = reservationIdValue
		};
	}
}
