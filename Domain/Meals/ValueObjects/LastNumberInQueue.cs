﻿using Domain.Customers.Exceptions;
using Domain.Meals.Exceptions;
using SharedKernal.ValueObjects;

namespace Domain.Meals.ValueObjects;
public class LastNumberInQueue : ValueObject<int>
{
	public LastNumberInQueue(int value) : base(value) { }

	protected override void Validate(int value)
	{
		if(value < 0)
		{
			throw new NegativeLastNumberInQueueException();
		}
	}
}
