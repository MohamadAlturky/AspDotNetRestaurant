using Domain.Pricing.Aggregate;
using Domain.Shared.Repositories;

namespace Domain.Shared.Proxies;
public class PricingRepositoryProxy
{
	private readonly IPricingRepository _pricingRepository;

	public PricingRepositoryProxy(IPricingRepository pricingRepository)
	{
		_pricingRepository = pricingRepository;
	}
	public PricingRecord? GetPriceByCustomerTypeJoinMealType(string customerType, string mealType)
	{
		return _pricingRepository.GetPriceByCustomerTypeJoinMealType(customerType, mealType);
	}
}
