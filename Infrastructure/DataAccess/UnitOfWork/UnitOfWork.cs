using Domain.Localization;
using Infrastructure.DataAccess.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernal.DomainEvents;
using SharedKernal.Entities;
using SharedKernal.Repositories;
using System.Data;
using System.Threading;

namespace Infrastructure.DataAccess.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
	private readonly RestaurantContext _context;

	//private readonly IPublisher _publisher;

	//public UnitOfWork(RestaurantContext context, IPublisher publisher)
	//{
	//	_context = context;
	//	_publisher = publisher;
	//}
	public UnitOfWork(RestaurantContext context)
	{
		_context = context;
	}

	public IDbTransaction BeginTransaction()
	{
		IDbContextTransaction transaction = _context.Database.BeginTransaction();
		return transaction.GetDbTransaction();
	}
	public void EndTransaction()
	{
		_context.Database.CloseConnection();
	}


	public void SaveChanges()
	{
		_context.SaveChanges();
	}

	public async Task SaveChangesAsync()
	{
		//var domainEvents = _context.ChangeTracker
		//	.Entries<AggregateRoot>()
		//	.Select(tracker => tracker.Entity)
		//	.SelectMany(aggregateRoot =>
		//	{
		//		var domainEvents = aggregateRoot.DomainEvents;
		//		aggregateRoot.ClearDomainEvents();
		//		return domainEvents;
		//	})
		//	.ToList();
		//foreach (var domainEvent in domainEvents)
		//{
		//	_publisher.Publish(domainEvent);
		//}
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DBConcurrencyException)
		{
			throw new Exception(LocalizationProvider.GetResource(DomainResourcesKeys.DBConcurrencyException));
		}
		catch (DbUpdateConcurrencyException)
		{
			throw new Exception(LocalizationProvider.GetResource(DomainResourcesKeys.DBConcurrencyException));
		}
		catch (Exception)
		{
			throw new Exception("حدثت مشكلة في قاعدة المعطيات حاول ثانيةً");
		}
	}
}
