using SharedResources.LocalizationProviders;
using SharedResources.RecourcesKeys;

namespace Domain.MealEntries.Exceptions;
public class NegativeLastNumberInQueueException : Exception
{
	public NegativeLastNumberInQueueException()
		: base(LocalizationProvider
			.GetResource(DomainResourcesKeys
				.NegativeLastNumberInQueueException))
	{ }
}
