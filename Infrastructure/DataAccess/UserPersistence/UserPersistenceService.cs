using Infrastructure.Authentication.Models;
using Infrastructure.Authentication.PasswordHashing;
using Infrastructure.DataAccess.DBContext;
using Infrastructure.ForgetPasswordHandling.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernal.Utilities.Result;
using System.Linq;

namespace Infrastructure.DataAccess.UserPersistence;

public class UserPersistenceService : IUserPersistenceService
{
	private readonly RestaurantContext _context;
	private readonly IHashHandler _hashHandler;

	public UserPersistenceService(RestaurantContext context, IHashHandler hashHandler)
	{
		_context = context;
		_hashHandler = hashHandler;
	}

	public void AddRolesToUser(long userId, List<Role> roles)
	{
		foreach (var role in roles)
		{
			_context.Set<UserRole>().Add(new UserRole()
			{
				UserId = userId,
				RoleId = role.Id
			});
		}
	}

	public async Task ChangePassword(long userId, string oldPassword, string newPassword)
	{
		User? user = _context.Set<User>().FirstOrDefault(user => user.Id == userId);

		if (user is null)
		{
			throw new Exception("if(user is null)");
		}

		string oldHashedPassword = _hashHandler.GetHash(oldPassword);

		if (oldHashedPassword != user.HashedPassword)
		{
			throw new Exception("if (hashedPassword != user.HashedPassword)");
		}

		string newHashedPassword = _hashHandler.GetHash(newPassword);
		user.HashedPassword = newHashedPassword;

		_context.Set<User>().Update(user);
		await _context.SaveChangesAsync();
	}

	public Result CheckPasswordValidity(int serialNumber, string password)
	{
		string hashedPassword = _hashHandler.GetHash(password);
		User? user = _context.Set<User>()
			.Include(user => user.Customer)
			.Where(user => user.Customer.SerialNumber == serialNumber)
			.FirstOrDefault();
		if (user is null)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", "if(user is null)"));
		}
		if (hashedPassword != user.HashedPassword)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", "if(hashedPassword != user.HashedPassword)"));
		}
		return Result.Success();
	}

	public void CreateUser(User user)
	{
		_context.Set<User>().Add(user);
	}

	public ForgetPasswordEntry? GetForgetPasswordEntryOnThisDay(long userId)
	{
		DateTime toDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
		DateTime tomorrow = toDay.AddDays(1);

		return _context.Set<ForgetPasswordEntry>()
					.Where(entry => entry.UserId == userId)
					.Where(entry => entry.AtDay<tomorrow&&entry.AtDay>=toDay)
					.FirstOrDefault();
	}

	public User? GetUser(int serialNumber)
	{
		return _context.Set<User>()
			.Include(user => user.Customer)
			.Where(user => user.Customer.SerialNumber == serialNumber)
			.FirstOrDefault();
	}

	public async Task<User?> GetUserAsync(int serialNumber)
	{
		return await _context.Set<User>()
			.Include(user => user.Customer)
			.Where(user => user.Customer.SerialNumber == serialNumber)
			.FirstOrDefaultAsync();
	}

	public void UpdateUserPassword(int serialNumber, string password)
	{
		User? user = _context.Set<User>()
			.Where(user => user.Customer.SerialNumber == serialNumber)
			.FirstOrDefault();

		if (user is null)
		{
			throw new Exception("if(user is null)");
		}
		user.HashedPassword = _hashHandler.GetHash(password);

		_context.Set<User>().Update(user);
		_context.SaveChanges();
	}
}
