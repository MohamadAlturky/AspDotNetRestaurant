using Infrastructure.Authentication.PasswordHashing;
using Infrastructure.DataAccess.DatabaseSeeding.SeedSuperUsers;
using Infrastructure.DataAccess.DatabaseSeeding.SeedPricingRecords;
using Infrastructure.DataAccess.DBContext;

namespace Presentation.DataBaseSeedingExtension;

public static class DataBaseSeeding
{
	public static async Task EnsureDataCompleteness(this WebApplication application)
	{
		using var scope = application.Services.CreateScope();
		RestaurantContext dbContext = scope.ServiceProvider.GetRequiredService<RestaurantContext>();
		IHashHandler hashHandler = new HashHandler();
		ISeedSuperUsersService adminSeeder = new SeedSuperUsersService(dbContext, hashHandler);
		ISeedPricingService pricingSeeder = new SeedPricingService(dbContext);

		await adminSeeder.SeedSuperUsers();
		await pricingSeeder.SeedPricing();
	}
}
