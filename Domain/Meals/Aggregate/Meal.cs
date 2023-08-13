using Domain.Meals.ValueObjects;
using Domain.Shared.Entities;
using Domain.Shared.ValueObjects;
using SharedKernal.Entities;

namespace Domain.Meals.Aggregate;
public class Meal : AggregateRoot
{
	private MealType _type = MealType.UnDefined;
	private NumberOfCalories _numberOfCalories = new NumberOfCalories(0);
	private MealDescription _description = new MealDescription("");


	public int NumberOfCalories { get => _numberOfCalories.Value; set => _numberOfCalories = new NumberOfCalories(value); }
	public string Type { get => _type.ToString(); set => _type = Enum.Parse<MealType>(value); }
	public string Description { get => _description.Value; set => _description = new MealDescription(value); }
	public ICollection<MealEntry> MealEntries { get; set; } = new HashSet<MealEntry>();



	public string ImagePath { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;



	public Meal(long id) : base(id) { }
	public Meal() : base(0) { }



	// Methods
	public void PrepareNewEntry(DateTime atDay, int preparedCount, bool hasAnEntryInTheSameDate)
	{
		if (hasAnEntryInTheSameDate)
		{
			throw new Exception("hasAnEntryInTheSameDate");
		}
		MealEntries.Add(new MealEntry(0, this.Id)
		{

			PreparedCount = preparedCount,
			AtDay = atDay
		});
	}

	public static Meal Create(int numberOfCalories,
		string type,
		string description,
		string imagePath,
		string name,
		long id = 0)
	{

		return new Meal(id)
		{
			NumberOfCalories = numberOfCalories,
			Type = type,
			Description = description,
			ImagePath = imagePath,
			Name = name,
		};
	}
}
