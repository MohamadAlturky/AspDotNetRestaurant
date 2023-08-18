using Domain.Customers.Aggregate;
using Domain.Customers.Exceptions;
using Domain.Customers.Repositories;
using Infrastructure.DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using SharedKernal.Entities;

namespace Infrastructure.CustomersPersistance.Repository;
public class CustomerRepository : ICustomerRepository
{
	private readonly RestaurantContext _context;

	public CustomerRepository(RestaurantContext context)
	{
		_context = context;
	}



	public void Add(Customer Entity)
	{
		Entity.Id = 0;
		
		var customer = _context.Set<Customer>().FirstOrDefault(customer => customer.SerialNumber == Entity.SerialNumber);
		
		if (customer is not null)
		{
			throw new SomeOneHasTheSameSerialNumberException();
		}

		_context.Set<Customer>().Add(Entity);
	}

	public long CalculateSumOfBalances()
	{
		return _context.Set<Customer>().Select(customer => customer.Balance).Sum();
	}



	public void Delete(Customer Entity)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<Customer> GetAll()
	{
			return _context.Set<Customer>()
				.AsNoTracking()
				.OrderByDescending(customer => customer.Id)
				.ToList();
	}

	public Customer? GetById(long id)
	{
		return _context.Set<Customer>().Find(id);
	}

	public Customer? GetBySerialNumber(int serialNumber)
	{
		return _context.Set<Customer>()
			.Where(customer => customer.SerialNumber == serialNumber)
			.AsNoTracking()
			.SingleOrDefault();
	}

	public IEnumerable<Customer> GetPage(int pageSize, int pageNumber)
	{
		return _context.Set<Customer>()
			.OrderByDescending(customer => customer.Id)
			.AsNoTracking()
			.Skip(pageSize * (pageNumber - 1))
			.Take(pageSize).ToList();
	}

	public void Update(Customer Entity)
	{
		_context.Set<Customer>().Update(Entity);
	}

	public void UpdateAll(List<Customer> customers)
	{
		_context.Set<Customer>().UpdateRange(customers);
	}
}
