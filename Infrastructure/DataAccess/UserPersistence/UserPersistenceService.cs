using Infrastructure.Authentication.Models;
using Infrastructure.DataAccess.DBContext;

namespace Infrastructure.DataAccess.UserPersistence;

public class UserPersistenceService : IUserPersistenceService
{
	private readonly RestaurantContext _context;

	public UserPersistenceService(RestaurantContext context)
	{
		_context = context;
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

	public void CreateUser(User user)
	{
		_context.Set<User>().Add(user);
	}
}
