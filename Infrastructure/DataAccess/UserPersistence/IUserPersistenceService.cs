using Infrastructure.Authentication.Models;

namespace Infrastructure.DataAccess.UserPersistence;
public interface IUserPersistenceService
{
	void CreateUser(User user);
	void AddRolesToUser(long userId, List<Role> roles);
}
