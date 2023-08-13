using Infrastructure.Authentication.PasswordHashing;
using Infrastructure.DataAccess.DatabaseSeeding.SeedAdmin;
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
		ISeedAdminService adminSeeder = new SeedAdminService(dbContext, hashHandler);
		ISeedPricingService pricingSeeder = new SeedPricingService(dbContext);

		await adminSeeder.SeedAdmin();
		await pricingSeeder.SeedPricing();
	}
}
