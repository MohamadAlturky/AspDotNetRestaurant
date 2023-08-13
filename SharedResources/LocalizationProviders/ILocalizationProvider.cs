using Microsoft.Extensions.Localization;

namespace SharedResources.LocalizationProviders;
public interface ILocalizationProvider
{
	IStringLocalizer<SharedResourcesProvider> GetLocalizer();
}
