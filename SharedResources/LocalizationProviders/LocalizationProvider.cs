using Microsoft.Extensions.Localization;

namespace SharedResources.LocalizationProviders;
public static class LocalizationProvider
{
	private static IStringLocalizer<SharedResourcesProvider>? _localizer;

	internal static void SetLocalizer(IStringLocalizer<SharedResourcesProvider> localizer)
	{
		_localizer = localizer;
	}

	public static IStringLocalizer<SharedResourcesProvider> GetLocalizer()
	{
		if( _localizer == null)
		{
			throw new ApplicationException("IStringLocalizer<DomainResources> GetLocalizer()");
		}
		return _localizer;
	}
	public static string GetResource(string resourceKey)
	{
		if (_localizer == null)
		{
			throw new ApplicationException("IStringLocalizer<DomainResources> GetLocalizer()");
		}
		return _localizer[resourceKey].Value;
	}
}
