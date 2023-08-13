using SharedResources.LocalizationProviders;
using SharedResources.RecourcesKeys;

namespace Domain.Reservations.Exceptions;
public class NegativeNumberInQueueException : Exception
{
	public NegativeNumberInQueueException() 
		: base(LocalizationProvider
			.GetResource(DomainResourcesKeys
				.NegativeNumberInQueueException)) { }
}
