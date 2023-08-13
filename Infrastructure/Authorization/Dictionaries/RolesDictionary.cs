using Infrastructure.Authentication.Models;


namespace Infrastructure.Authorization.Dictionaries;
public static class RolesDictionary
{
	public static Role Manager => new()
	{
		Id = 1,
		Name = "Manager"
	};

	public static Role User => new()
	{
		Id = 2,
		Name = "User"
	};
}
