using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRepositorySoftDelete<Trip>, TripRepository>();
        services.AddScoped<IRepositorySoftDelete<Transit>, TransitRepository>();
        services.AddScoped<IRepositorySoftDelete<Train>, TrainRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IStationRepository, StationRepository>();
        services.AddScoped<IRepositorySoftDelete<RouteTransit>, RouteTransitRepository>();
        services.AddScoped<IRepositorySoftDelete<Route>, RouteRepository>();
        services.AddScoped<IRepositorySoftDelete<Platform>, PlatformRepository>();
        services.AddScoped<IRepositorySoftDelete<PlatformMaintenance>, PlatformMaintenanceRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRepositorySoftDelete<CarriageType>, CarriageTypeRepository>();
        services.AddScoped<IRepositorySoftDelete<Carriage>, CarriageRepository>();
        services.AddScoped<ITripStationRepository, TripStationRepository>();
        services.AddScoped<ILoginUser, UserRepository>();
    }

    public static void AddServicesScoped(this IServiceCollection services)
    {
        //Services
        services.AddScoped<ICarriageTypeService, CarriageTypeService>();
        services.AddScoped<IStationService, StationService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRouteService, RouteService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ITrainService, TrainService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ITransitService, TransitService>();
        services.AddScoped<ITripStationService, TripStationService>();
        services.AddScoped<ICarriageService, CarriageService>();
        services.AddScoped<ITripService, TripService>();
        services.AddScoped<IOrderService, OrderService>();
    }

    public static void AddSwaggerGenWithOptions(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test01", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."

            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
        });
    }
}