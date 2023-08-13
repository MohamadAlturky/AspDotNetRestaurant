using Domain.Customers.Aggregate;
using SharedKernal.Repositories;

namespace Domain.Customers.Repositories;
public interface ICustomerRepository : IRepository<Customer>
{
	Customer? GetBySerialNumber(int serialNumber);
	long CalculateSumOfBalances();
}
