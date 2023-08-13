using Infrastructure.Authentication.Models;

namespace Infrastructure.Authorization.Dictionaries;
public static class PermissionsDictionary
{
	public static Permission ReadContent => new()
	{
		Id = 1,
		Name = "ReadContent"
	};

	public static Permission RegisterCustomer => new()
	{
		Id = 2,
		Name = "RegisterCustomer"
	};

	public static Permission CreateContent => new()
	{
		Id = 3,
		Name = "CreateContent"
	};

	public static Permission OrderContent => new()
	{
		Id = 4,
		Name = "OrderContent"
	};

	public static Permission ReadSystemInfo => new()
	{
		Id = 5,
		Name = "ReadSystemInfo"
	};

}
