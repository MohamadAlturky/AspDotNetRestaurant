using Infrastructure.Authentication.Models;
using Infrastructure.Authentication.PasswordHashing;
using Infrastructure.DataAccess.DBContext;

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

	public void CreateUser(User user)
	{
		_context.Set<User>().Add(user);
	}
}
