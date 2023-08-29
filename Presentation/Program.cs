using Infrastructure.DataAccess.DBContext;
using Infrastructure.DataAccess.Interceptors;
using Infrastructure.DependencyInjectionConfiguration;
using Infrastructure.Notification.Hubs;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Presentation.DataBaseSeedingExtension;
using Presentation.DependencyInjectionConfiguration;
using Serilog;
using Localization.DependencyInjectionConfiguration;
using System.Globalization;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<IISServerOptions>(options =>
{
	options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.ConfigureSystemOptions();
builder.Services.AddInfrastructure();
builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddLocalizationBuilders();

builder.Services.AddScoped(typeof(NotificationsHub));
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<AudtingSaveChangesInterceptor>();
// Persistance level
//builder.Services.AddDbContext<RestaurantContext>((serviceProvider, option) =>
//{
//	DomainEventsCollectorInterceptor? interceptor
//	= serviceProvider.GetService<DomainEventsCollectorInterceptor>();
//	if (interceptor is null
//		|| connectionString is null)
//	{
//		throw new ApplicationException();
//	}
//	option.UseSqlServer(connectionString).AddInterceptors(interceptor);
//});


builder.Services.AddDbContext<RestaurantContext>((serviceProvider, option) =>
{
	AudtingSaveChangesInterceptor? audtingSaveChangesInterceptor =
		serviceProvider.GetService<AudtingSaveChangesInterceptor>();
	if (audtingSaveChangesInterceptor is null)
	{
		throw new ApplicationException();
	}
	if (connectionString is null)
	{
		throw new ApplicationException();
	}
	option.UseSqlServer(connectionString).AddInterceptors(audtingSaveChangesInterceptor);
});

// CORS
var AllowSpecificOrigins = "_AllowFrontEnd";
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllowSpecificOrigins,
					  policy =>
					  {
						  //policy.WithOrigins("*")
						  policy.WithOrigins("http://localhost:5173")
						  .AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowCredentials();
					  });
});





//images
builder.Services.AddDirectoryBrowser();


// Serilog
builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration));



// localiztion

builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(opt =>
{
	opt.ResourcesPath = "";
});

builder.Services.AddScoped<GlobalExceptionHandler>();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	List<CultureInfo> supportedCultures = new List<CultureInfo>
{
			//new CultureInfo("en-US"),
			new CultureInfo("ar-SY")
};

	options.DefaultRequestCulture = new RequestCulture("ar-SY");
	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;
});


//


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

await app.EnsureDataCompleteness();

await app.BuildLocalizationRequirements();

// localization middleware
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();

if (options == null)
{
	throw new DllNotFoundException();
}

app.UseRequestLocalization(options.Value);



app.UseHttpsRedirection();
app.UseCors(AllowSpecificOrigins);
app.UseStaticFiles();

var fileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.WebRootPath, "MealsImages"));
var requestPath = "/MealsImages";

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = fileProvider,
	RequestPath = requestPath
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
	FileProvider = fileProvider,
	RequestPath = requestPath
});






app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();
app.MapControllers();
app.MapHub<NotificationsHub>("/notifications");
app.Run();