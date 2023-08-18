﻿using Domain.Customers.Aggregate;
using Domain.Customers.ValueObjects;
using Infrastructure.Authentication.Models;
using Infrastructure.Authentication.PasswordHashing;
using Infrastructure.Authorization.Dictionaries;
using Infrastructure.DataAccess.DBContext;
using SharedKernal.Utilities.Result;

namespace Infrastructure.DataAccess.DatabaseSeeding.SeedSuperUsers;
public class SeedSuperUsersService : ISeedSuperUsersService
{
	private readonly RestaurantContext _context;
	private readonly IHashHandler _hashHandler;

	private readonly string ADMIN_PASSWORD = "admin";
	private readonly string CONSUMER_PASSWORD = "admin";
	private readonly string ACCOUNTANT_PASSWORD = "admin";

	private readonly User ADMIN = new User()
	{
		Customer = new Customer()
		{
			SerialNumber = 0,
			Balance = 0,
			BelongsToDepartment = Department.Center.ToString(),
			Category = CustomerType.RestautantManager.ToString(),
			FirstName = "مدير",
			LastName = "المطعم",
			Notes = "",
			IsRegular = true,
			Eligible = true,
			IsActive = true
		},
		Id = 1,
		Roles = new Role[]
		{
			RolesDictionary.Manager
		}
	};
	private readonly User CONSUMER = new User()
	{
		Customer = new Customer()
		{
			SerialNumber = 1,
			Balance = 0,
			BelongsToDepartment = Department.Center.ToString(),
			Category = CustomerType.ReservationsConsumer.ToString(),
			FirstName = "حساب استهلاك الطعام",
			LastName = "",
			Notes = "",
			IsRegular = true,
			Eligible = true,
			IsActive = true
		},
		Id = 2,
		Roles = new Role[]
		{
			RolesDictionary.Consumer
		}
	};
	private readonly User ACCOUNTANT = new User()
	{
		Customer = new Customer()
		{
			SerialNumber = 2,
			Balance = 0,
			BelongsToDepartment = Department.Center.ToString(),
			Category = CustomerType.Accountant.ToString(),
			FirstName = "المحاسب",
			LastName = "",
			Notes = "",
			IsRegular = true,
			Eligible = true,
			IsActive = true
		},
		Id = 3,
		Roles = new Role[]
		{
			RolesDictionary.Accountant
		}
	};
	public SeedSuperUsersService(RestaurantContext context, IHashHandler hashHandler)
	{
		_context = context;
		_hashHandler = hashHandler;
	}

	public async Task<Result> SeedSuperUsers()
	{
		using var transaction = _context.Database.BeginTransaction();
		try
		{
			User? admin = await _context.Set<User>().FindAsync(ADMIN.Id);
			User? consumer = await _context.Set<User>().FindAsync(CONSUMER.Id);
			User? accountant = await _context.Set<User>().FindAsync(ACCOUNTANT.Id);

			if (admin is null)
			{
				var adminRole = _context.Set<Role>().Find(RolesDictionary.Manager.Id);
				
				if(adminRole is null)
				{
					throw new Exception();
				}
				ADMIN.HashedPassword = _hashHandler.GetHash(ADMIN_PASSWORD);
				ADMIN.Roles = new Role[]
				{
					adminRole
				};

				_context.Set<User>().Add(ADMIN);
			}
			if(consumer is null)
			{
				var consumerRole = _context.Set<Role>().Find(RolesDictionary.Consumer.Id);
				
				if(consumerRole is null)
				{
					throw new Exception();
				}
					CONSUMER.HashedPassword = _hashHandler.GetHash(CONSUMER_PASSWORD);
				CONSUMER.Roles = new Role[]
				{
					consumerRole
				};
				_context.Set<User>().Add(CONSUMER);
			}
			if (accountant is null)
			{
				var accountantRole = _context.Set<Role>().Find(RolesDictionary.Accountant.Id);

				if(accountantRole is null)
				{
					throw new Exception();
				}

				ACCOUNTANT.HashedPassword = _hashHandler.GetHash(ACCOUNTANT_PASSWORD);
				ACCOUNTANT.Roles = new Role[]
				{
					accountantRole
				};
				_context.Set<User>().Add(ACCOUNTANT);
			}

			
			await _context.SaveChangesAsync();
			transaction.Commit();
			return Result.Success();
		}
		catch (Exception exception)
		{
			transaction.Rollback();
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", exception.Message));
		}
	}
}
