using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpaceZD.API.Middleware;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;
using Route = SpaceZD.DataLayer.Entities.Route;

const string connectionEnvironmentVariableName = "CONNECTIONS_STRING";
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetValue<string>(connectionEnvironmentVariableName);
builder.Services.AddDbContext<VeryVeryImportantContext>(op => op.UseLazyLoadingProxies().UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(BusinessLayerMapper).Assembly);

//Repositories
builder.Services.AddScoped<IRepositorySoftDelete<User>, UserRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Trip>, TripRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Transit>, TransitRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Train>, TrainRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Ticket>, TicketRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<Station>, StationRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<RouteTransit>, RouteTransitRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<Route>, RouteRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Platform>, PlatformRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<PlatformMaintenance>, PlatformMaintenanceRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Person>, PersonRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Order>, OrderRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<CarriageType>, CarriageTypeRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Carriage>, CarriageRepository>();
builder.Services.AddScoped<IRepository<TripStation>, TripStationRepository>();
builder.Services.AddScoped<ILoginUser, UserRepository>();

//Services
builder.Services.AddScoped<ICarriageTypeService, CarriageTypeService>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRouteService, RouteService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<SpaceZdMiddleware>();

app.MapControllers();

app.Run();