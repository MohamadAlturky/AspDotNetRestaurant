using SharedResources.LocalizationProviders;
using SharedResources.RecourcesKeys;

namespace Domain.Reservations.Exceptions;
public class NegativeNumberOfReservationsException : Exception
{
	public NegativeNumberOfReservationsException()
		: base(LocalizationProvider
			.GetResource(DomainResourcesKeys
				.NegativeNumberOfReservationsException))
	{ }
}
