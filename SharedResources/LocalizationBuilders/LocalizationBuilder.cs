using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SharedResources.LocalizationProviders;

namespace SharedResources.LocalizationBuilders;
public class LocalizationBuilder
{
	private readonly IServiceScopeFactory _serviceScopeFactory;

	public LocalizationBuilder(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}


	public void Build()
	{	
		using IServiceScope scope = _serviceScopeFactory.CreateScope();

		IStringLocalizer<SharedResourcesProvider> localizer = scope.ServiceProvider
			.GetRequiredService<IStringLocalizer<SharedResourcesProvider>>();

		LocalizationProvider.SetLocalizer(localizer);
	}
}
