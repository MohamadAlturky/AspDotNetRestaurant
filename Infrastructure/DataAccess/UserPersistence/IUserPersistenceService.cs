using Infrastructure.Authentication.Models;
using SharedKernal.Utilities.Result;

namespace Infrastructure.DataAccess.UserPersistence;
public interface IUserPersistenceService
{
	void CreateUser(User user);
	void AddRolesToUser(long userId, List<Role> roles);
	Task ChangePassword(long userId,string oldPassword,string newPassword);
	Result CheckPasswordValidity(int serialNumber, string password);
	User? GetUser(int serialNumber);
}
