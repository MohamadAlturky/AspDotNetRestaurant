using Infrastructure.Authentication.Models;
using Infrastructure.Authorization.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configurations;
public class PermissionsConfiguration : IEntityTypeConfiguration<Permission>
{
	public void Configure(EntityTypeBuilder<Permission> builder)
	{
		builder.HasKey(permission => permission.Id);

		IEnumerable<Permission> permissions = new List<Permission>()
		{
			PermissionsDictionary.RegisterCustomer,
			PermissionsDictionary.OrderContent,
			PermissionsDictionary.CreateContent,
			PermissionsDictionary.ReadContent,
			PermissionsDictionary.ReadSystemInfo
		};

		builder.HasData(permissions);
	}
}
