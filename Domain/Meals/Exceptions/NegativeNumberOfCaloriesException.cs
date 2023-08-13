using SharedResources.LocalizationProviders;
using SharedResources.RecourcesKeys;

namespace Domain.Meals.Exceptions;
public class NegativeNumberOfCaloriesException : Exception
{
	public NegativeNumberOfCaloriesException() 
		: base(LocalizationProvider
			.GetResource(DomainResourcesKeys
				.NegativeNumberOfCaloriesException)) { }
}
