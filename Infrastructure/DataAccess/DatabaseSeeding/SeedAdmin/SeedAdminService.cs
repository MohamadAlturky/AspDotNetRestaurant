using Domain.Customers.Aggregate;
using Domain.Customers.ValueObjects;
using Infrastructure.Authentication.Models;
using Infrastructure.Authentication.PasswordHashing;
using Infrastructure.Authorization.Dictionaries;
using Infrastructure.DataAccess.DBContext;
using SharedKernal.Utilities.Result;

namespace Infrastructure.DataAccess.DatabaseSeeding.SeedAdmin;
public class SeedAdminService : ISeedAdminService
{
	private readonly RestaurantContext _context;
	private readonly IHashHandler _hashHandler;
	private readonly string ADMIN_PASSWORD = "admin";
	public SeedAdminService(RestaurantContext context, IHashHandler hashHandler)
	{
		_context = context;
		_hashHandler = hashHandler;
	}

	public async Task<Result> SeedAdmin()
	{
		using var transaction = _context.Database.BeginTransaction();
		try
		{
			User? admin = await _context.Set<User>().FindAsync((long)1);

			if (admin is not null)
			{
				throw new Exception("مدير النظام موجود بلفعل إذا نسيت كلمة السر فهذه مشكلتك.");
			}

			Customer customer = new Customer()
			{
				SerialNumber = 0,
				Balance = 0,
				BelongsToDepartment = Department.Center.ToString(),
				Category = CustomerType.RestautantManager.ToString(),
				FirstName = "محمد",
				LastName = "التركي",
				Notes = "ملك المشاريع",
				IsRegular = true,
				Eligible = true,
				IsActive = true
			};

			//_context.Set<Customer>().Add(customer);
			//_context.SaveChanges();

			Role? role = _context.Set<Role>().Find(RolesDictionary.Manager.Id);

			if (role is null)
			{
				throw new Exception("role is null");
			}
			_context.Set<User>().Add(new User()
			{
				Customer = customer,
				HashedPassword = _hashHandler.GetHash(ADMIN_PASSWORD),
				Roles = new List<Role>()
				{
					role
				}
			});
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
