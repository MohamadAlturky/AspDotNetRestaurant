﻿using SharedResources.LocalizationProviders;
using SharedResources.RecourcesKeys;

namespace Domain.Customers.Exceptions;
public class SomeOneHasTheSameSerialNumberException : Exception
{
	public SomeOneHasTheSameSerialNumberException() 
		: base(LocalizationProvider.GetResource(DomainResourcesKeys.SomeOneHasTheSameSerialNumberException))
	{
		
	}
}
