using Domain.Customers.Aggregate;
using Domain.Customers.Repositories;

namespace Domain.Shared.Proxies;
public class CustomerRepositoryProxy
{
	private readonly ICustomerRepository _customerRepository;

	public CustomerRepositoryProxy(ICustomerRepository customerRepository)
	{
		_customerRepository = customerRepository;
	}
	public Customer? GetCustomerById(long id)
	{
		return _customerRepository.GetById(id);
	}
}
