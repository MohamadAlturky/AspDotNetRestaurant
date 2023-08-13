using Domain.Meals.Exceptions;
using SharedKernal.ValueObjects;

namespace Domain.Shared.ValueObjects;
public class NumberOfCalories : ValueObject<int>
{
	public NumberOfCalories(int value) : base(value)
	{
	}

	protected override void Validate(int value)
	{
		if (value < 0)
		{
			throw new NegativeNumberOfCaloriesException();
		}
	}
}
