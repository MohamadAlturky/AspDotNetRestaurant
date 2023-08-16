using Infrastructure.DataAccess.DBContext;
using Infrastructure.DataAccess.Interceptors;
using Infrastructure.DependencyInjectionConfiguration;
using Infrastructure.Notification;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Presentation.DataBaseSeedingExtension;
using Presentation.DependencyInjectionConfiguration;
using Serilog;
using SharedResources.DependencyInjectionConfiguration;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<IISServerOptions>(options =>
{
	options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.AddInfrastructure();
builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddLocalizationBuilders();

builder.Services.AddScoped(typeof(NotificationsHub));
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Persistance level
builder.Services.AddDbContext<RestaurantContext>((serviceProvider, option) =>
{
	DomainEventsCollectorInterceptor? interceptor
	= serviceProvider.GetService<DomainEventsCollectorInterceptor>();
	if (interceptor is null
		|| connectionString is null)
	{
		throw new ApplicationException();
	}
	option.UseSqlServer(connectionString).AddInterceptors(interceptor);
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

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	List<CultureInfo> supportedCultures = new List<CultureInfo>
{
			new CultureInfo("en-US"),
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
	await app.EnsureDataCompleteness();
	app.UseSwagger();
	app.UseSwaggerUI();
}

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

// Enable displaying browser links.
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

//app.UseEndpoints(endpoint =>
//{
//	endpoint.MapControllers();
//	endpoint.MapHub<NotificationsHub>("/notifications");
//});

app.MapControllers();
app.MapHub<NotificationsHub>("/notifications");
app.Run();