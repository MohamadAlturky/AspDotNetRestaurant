using Infrastructure.DataAccess.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernal.DomainEvents;
using SharedKernal.Entities;
using SharedKernal.Repositories;
using System.Data;

namespace Infrastructure.DataAccess.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
	private readonly RestaurantContext _context;

	private readonly IPublisher _publisher;

	public UnitOfWork(RestaurantContext context, IPublisher publisher)
	{
		_context = context;
		_publisher = publisher;
	}

	public IDbTransaction BeginTransaction()
	{
		IDbContextTransaction transaction = _context.Database.BeginTransaction();
		return transaction.GetDbTransaction();
	}

	public void SaveChanges()
	{
		_context.SaveChanges();
	}

	public Task SaveChangesAsync()
	{
		var domainEvents = _context.ChangeTracker
			.Entries<AggregateRoot>()
			.Select(tracker => tracker.Entity)
			.SelectMany(aggregateRoot =>
			{
				var domainEvents = aggregateRoot.DomainEvents;
				aggregateRoot.ClearDomainEvents();
				return domainEvents;
			})
			.ToList();
		foreach (var domainEvent in domainEvents)
		{
			_publisher.Publish(domainEvent);
		}

		return _context.SaveChangesAsync();
	}
}
