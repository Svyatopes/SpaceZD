using Microsoft.EntityFrameworkCore;
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

var connectionString = builder.Configuration.GetValue<string>(connectionEnvironmentVariableName);
builder.Services.AddDbContext<VeryVeryImportantContext>(op => op.UseLazyLoadingProxies().UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(BusinessLayerMapper).Assembly);

//Repositories

builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<User>,UserRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Trip>,TripRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<Transit>,TransitRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<Train>,TrainRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<Ticket>,TicketRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<Station>,StationRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<RouteTransit>,RouteTransitRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<Route>,RouteRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<Platform>,PlatformRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<PlatformMaintenance>,PlatformMaintenanceRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<Person>,PersonRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<Order>,OrderRepository>();
builder.Services.AddScoped<IRepositorySoftDeleteNewUpdate<CarriageType>,CarriageTypeRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Carriage>,CarriageRepository>();
builder.Services.AddScoped<IRepository<TripStation>,TripStationRepository>();

//Services
builder.Services.AddScoped<ICarriageTypeService, CarriageTypeService>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<SpaceZdMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
