using SharedResources.LocalizationProviders;
using SharedResources.RecourcesKeys;

namespace Domain.MealEntries.Exceptions;

public class NegativeNumberOfMealsException : Exception
{
	public NegativeNumberOfMealsException()
		: base(LocalizationProvider
			.GetResource(DomainResourcesKeys
				.NegativeNumberOfMealsException))
	{ }
}