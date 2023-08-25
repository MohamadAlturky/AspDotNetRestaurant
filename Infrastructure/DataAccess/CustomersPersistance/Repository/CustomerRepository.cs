using Domain.Customers.Aggregate;
using Domain.Customers.Entities;
using Domain.Customers.Exceptions;
using Domain.Customers.ReadModels;
using Domain.Customers.Repositories;
using Infrastructure.DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CustomersPersistance.Repository;
public class CustomerRepository : ICustomerRepository
{
	private readonly int TRANSACTION_PAGE_SIZE = 10;
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

	public void AddAccountTransaction(AccountTransaction accountTransaction)
	{
		_context.Set<AccountTransaction>().Add(accountTransaction);
	}

	public long CalculateSumOfBalances()
	{
		return _context.Set<Customer>().Select(customer => customer.Balance).Sum();
	}



	public void Delete(Customer Entity)
	{
		throw new NotImplementedException();
	}

	public AccountTransactionsReadModel? GetAccountTransactionsPage(int serialNumber, int pageNumber)
	{
		Customer? customer = _context.Set<Customer>().Where(customer => customer.SerialNumber == serialNumber).FirstOrDefault();

		if(customer is null)
		{
			throw new Exception("public AccountTransactionsReadModel? GetAccountTransactionsPage(int serialNumber, int pageNumber)");
		}
		IQueryable<AccountTransaction> transacion = _context.Set<AccountTransaction>()
			.Where(entry => entry.CustomerId == customer.Id)
			.OrderByDescending(entry=>entry.Id);
		int size = transacion.Count();
		var transactions = transacion
			.Skip(TRANSACTION_PAGE_SIZE * (pageNumber - 1))
			.Take(TRANSACTION_PAGE_SIZE)
			.Select(transaction=>new AccountTransactionReadModel()
			{
				Type = transaction.Type,
				Value=transaction.Value,
				CreatedAtDay=transaction.CreatedAt.ToString("dddd"),
				Date= transaction.CreatedAt.ToString("dd/MM/yyyy")
			})
			.ToList();
		AccountTransactionsReadModel model = new AccountTransactionsReadModel()
		{
			Size = size,
			TotalBalance = customer.Balance,
			AccountTransactions = transactions,
		};
		return model;
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
