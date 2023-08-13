using Microsoft.Extensions.DependencyInjection;
using SharedResources.LocalizationBuilders;

namespace SharedResources.DependencyInjectionConfiguration;
public static class DependencyInjection
{
	public static void AddLocalizationBuilders(this IServiceCollection services)
	{
		services.AddScoped(typeof(LocalizationBuilder));
	}
}
