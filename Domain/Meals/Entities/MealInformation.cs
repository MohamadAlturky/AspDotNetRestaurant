using Domain.Meals.Aggregate;
using Domain.Meals.ValueObjects;
using Domain.Shared.ValueObjects;
using SharedKernal.Entities;

namespace Domain.Meals.Entities;
public class MealInformation : Entity
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



	public MealInformation(long id) : base(id) { }
	public MealInformation() : base(0) { }



	// Methods
	public void PrepareNewEntry(DateTime atDay, int preparedCount, bool hasAnEntryInTheSameDate)
	{
		if (hasAnEntryInTheSameDate)
		{
			throw new Exception("hasAnEntryInTheSameDate");
		}
		MealEntries.Add(new MealEntry(0, Id)
		{

			PreparedCount = preparedCount,
			AtDay = atDay
		});
	}

	public static MealInformation Create(int numberOfCalories,
		string type,
		string description,
		string imagePath,
		string name,
		long id = 0)
	{

		return new MealInformation(id)
		{
			NumberOfCalories = numberOfCalories,
			Type = type,
			Description = description,
			ImagePath = imagePath,
			Name = name,
		};
	}
}
