using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;
using Route = SpaceZD.DataLayer.Entities.Route;

namespace SpaceZD.API.Extensions;

public static class BuilderServicesExtensions
{
    const string connectionEnvironmentVariableName = "CONNECTIONS_STRING";

    public static void AddAuthenticationExtension(this IServiceCollection services, string defaultScheme)
    {
        services.AddAuthentication(defaultScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });
    }

    public static void AddDbContextScoped(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetValue<string>(connectionEnvironmentVariableName);
        builder.Services.AddDbContext<VeryVeryImportantContext>(op =>
            op.UseLazyLoadingProxies().UseSqlServer(connectionString));
    }

    public static void AddRepositoriesScoped(this IServiceCollection services)
    {
        //Repositories
        services.AddScoped<IRepositorySoftDelete<User>, UserRepository>();
        services.AddScoped<IRepositorySoftDelete<Trip>, TripRepository>();
        services.AddScoped<IRepositorySoftDelete<Transit>, TransitRepository>();
        services.AddScoped<IRepositorySoftDelete<Train>, TrainRepository>();
        services.AddScoped<IRepositorySoftDelete<Ticket>, TicketRepository>();
        services.AddScoped<IStationRepository, StationRepository>();
        services.AddScoped<IRepositorySoftDelete<RouteTransit>, RouteTransitRepository>();
        services.AddScoped<IRepositorySoftDelete<Route>, RouteRepository>();
        services.AddScoped<IRepositorySoftDelete<Platform>, PlatformRepository>();
        services.AddScoped<IRepositorySoftDelete<PlatformMaintenance>, PlatformMaintenanceRepository>();
        services.AddScoped<IRepositorySoftDelete<Person>, PersonRepository>();
        services.AddScoped<IRepositorySoftDelete<Order>, OrderRepository>();
        services.AddScoped<IRepositorySoftDelete<CarriageType>, CarriageTypeRepository>();
        services.AddScoped<IRepositorySoftDelete<Carriage>, CarriageRepository>();
        services.AddScoped<IRepository<TripStation>, TripStationRepository>();
        services.AddScoped<ILoginUser, UserRepository>();
    }

    public static void AddServicesScoped(this IServiceCollection services)
    {
        //Services
        services.AddScoped<ICarriageTypeService, CarriageTypeService>();
        services.AddScoped<IStationService, StationService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRouteService, RouteService>();
    }
}