using SharedResources.LocalizationProviders;
using SharedResources.RecourcesKeys;

namespace Domain.Meals.Exceptions;
public class NegativeLastNumberInQueueException : Exception
{
	public NegativeLastNumberInQueueException() 
		: base(LocalizationProvider
			.GetResource(DomainResourcesKeys
				.NegativeLastNumberInQueueException)) { }
}
