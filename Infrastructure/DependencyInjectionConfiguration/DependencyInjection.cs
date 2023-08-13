using Domain.Customers.Repositories;
using Domain.Meals.Repositories;
using Domain.Reservations.Repositories;
using Domain.Shared.Repositories;
using Infrastructure.Authentication.JWTProvider;
using Infrastructure.Authentication.PasswordHashing;
using Infrastructure.Authorization.AuthorizationHandlers;
using Infrastructure.Authorization.AuthorizationPolicyProvider;
using Infrastructure.CustomersPersistance.Repository;
using Infrastructure.DataAccess.PermissionsService;
using Infrastructure.DataAccess.UnitOfWork;
using Infrastructure.DataAccess.UserPersistence;
using Infrastructure.MealsPersistence.Repository;
using Infrastructure.PricingRecordsPersistance.Repository;
using Infrastructure.ReservationsPersistence.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal.Repositories;

namespace Infrastructure.DependencyInjectionConfiguration;
public static class DependencyInjection
{
	public static void AddInfrastructure(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IMealRepository, MealRepository>();
		services.AddScoped<IReservationRepository, ReservationRepository>();
		services.AddScoped<ICustomerRepository, CustomerRepository>();
		services.AddScoped<IPricingRepository, PricingRepository>();
		services.AddScoped<IUserPersistenceService, UserPersistenceService>();
		services.AddScoped<IHashHandler, HashHandler>();
		services.AddScoped<IPermissionService, PermissionService>();
		services.AddScoped<IJwtProvider, JwtProvider>();
		services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
		services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
	}
}
