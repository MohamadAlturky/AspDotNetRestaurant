﻿using SharedResources.LocalizationProviders;
using SharedResources.RecourcesKeys;

namespace Domain.Reservations.Exceptions;
public class NegativePriceException : Exception
{
	public NegativePriceException()
		: base(LocalizationProvider
			.GetResource(DomainResourcesKeys
				.NegativePriceException)) { }
}
