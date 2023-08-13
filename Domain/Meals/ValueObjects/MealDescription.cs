using SharedKernal.ValueObjects;

namespace Domain.Meals.ValueObjects;
internal class MealDescription : ValueObject<string>
{
	public MealDescription(string value) : base(value) { }

	protected override void Validate(string value)
	{
		// until now no validation needed
	}
}
