﻿using Domain.Meals.Exceptions;
using SharedKernal.ValueObjects;

namespace Domain.Shared.ValueObjects;
public class NumberOfPreparedMeals : ValueObject<int>
{
	public NumberOfPreparedMeals(int value) : base(value)
	{ }

	protected override void Validate(int value)
	{
		if(value < 0)
		{
			throw new NegativeNumberOfMealsException();
		}
	}
}
