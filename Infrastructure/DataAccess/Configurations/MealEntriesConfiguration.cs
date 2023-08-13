using Domain.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configurations;
internal class MealEntriesConfiguration : IEntityTypeConfiguration<MealEntry>
{
	public void Configure(EntityTypeBuilder<MealEntry> builder)
	{
		#region Id
		builder.HasKey(meal => meal.Id);

		builder.HasIndex(meal => meal.Id).IsUnique();

		builder.Property(meal => meal.Id).ValueGeneratedOnAdd();
		#endregion


		#region MealId
		builder.Property(meal => meal.MealId);

		builder.HasIndex(meal => meal.MealId);

		//builder.HasOne(meal => meal.Meal).WithMany(meal=>meal.MealEntries).HasForeignKey(meal => meal.MealId);

		builder.HasOne(mealEntry => mealEntry.Meal)
				.WithMany(meal => meal.MealEntries)
				.HasForeignKey(mealEntry => mealEntry.MealId);
		#endregion

		#region AtDay

		builder.Property(meal => meal.AtDay);

		builder.HasIndex(meal => meal.AtDay);
		#endregion



		#region PreparedCount

		builder.Property(meal => meal.PreparedCount);

		#endregion



		#region LastNumberInQueue

		builder.Property(meal => meal.LastNumberInQueue);

		#endregion



		#region CustomerCanCancel

		builder.Property(meal => meal.CustomerCanCancel);

		#endregion


		#region ReservationsCount

		builder.Property(meal => meal.ReservationsCount);

		#endregion
	}
}
