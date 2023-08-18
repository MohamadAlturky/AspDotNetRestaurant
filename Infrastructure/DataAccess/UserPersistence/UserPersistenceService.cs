using Infrastructure.Authentication.Models;
using Infrastructure.Authentication.PasswordHashing;
using Infrastructure.DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using SharedKernal.Utilities.Result;

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
			.Include(user=>user.Customer)
			.Where(user => user.Customer.SerialNumber == serialNumber)
			.FirstOrDefault();
		if(user is null)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", "if(user is null)"));
		}
		if(hashedPassword != user.HashedPassword)
		{
			return Result.Failure(new SharedKernal.Utilities.Errors.Error("", "if(hashedPassword != user.HashedPassword)"));
		}
		return Result.Success();
	}

	public void CreateUser(User user)
	{
		_context.Set<User>().Add(user);
	}

	public User? GetUser(int serialNumber)
	{
		return _context.Set<User>()
			.Include(user=>user.Customer)
			.Where(user => user.Customer.SerialNumber == serialNumber)
			.FirstOrDefault();
	}
}
