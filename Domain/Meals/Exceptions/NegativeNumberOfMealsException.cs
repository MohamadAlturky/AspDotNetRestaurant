using SharedResources.LocalizationProviders;
using SharedResources.RecourcesKeys;

namespace Domain.Meals.Exceptions;

public class NegativeNumberOfMealsException : Exception
{
	public NegativeNumberOfMealsException() 
		: base(LocalizationProvider
			.GetResource(DomainResourcesKeys
				.NegativeNumberOfMealsException)) { }
}