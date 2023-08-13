using Infrastructure.Authentication.Models;
using Infrastructure.Authorization.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configurations;
public class RolesPermissionsConfiguration : IEntityTypeConfiguration<RolePermission>
{
	public void Configure(EntityTypeBuilder<RolePermission> builder)
	{
		builder.HasKey(entity => new { entity.RoleId, entity.PermissionId });

		IEnumerable<RolePermission> rolePermissions = new List<RolePermission>()
		{
			Create(RolesDictionary.Manager,PermissionsDictionary.ReadContent),
			Create(RolesDictionary.Manager,PermissionsDictionary.RegisterCustomer),
			Create(RolesDictionary.Manager,PermissionsDictionary.CreateContent),
			Create(RolesDictionary.Manager,PermissionsDictionary.OrderContent),
			Create(RolesDictionary.Manager,PermissionsDictionary.ReadSystemInfo),
			Create(RolesDictionary.User,PermissionsDictionary.ReadContent),
			Create(RolesDictionary.User,PermissionsDictionary.OrderContent)
		};

		builder.HasData(rolePermissions);
	}
	private static RolePermission Create(Role role, Permission permission)
	{
		return new RolePermission()
		{
			PermissionId = permission.Id,
			RoleId = role.Id
		};
	}
}
