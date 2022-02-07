using Microsoft.EntityFrameworkCore;
using SpaceZD.API.Configuration;
using SpaceZD.BusinessLayer.Configuration;
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

builder.Services.AddAutoMapper(
    typeof(BusinessLayerMapper),
    typeof(TicketApiMappingProfile),
    typeof(TrainApiMappingProfile),
    typeof(UserApiMappingProfile));

builder.Services.AddScoped<IRepositorySoftDelete<User>,UserRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Trip>,TripRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Transit>,TransitRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Train>,TrainRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Ticket>,TicketRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Station>,StationRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<RouteTransit>,RouteTransitRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Route>,RouteRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Platform>,PlatformRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<PlatformMaintenance>,PlatformMaintenanceRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Person>,PersonRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Order>,OrderRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<CarriageType>,CarriageTypeRepository>();
builder.Services.AddScoped<IRepositorySoftDelete<Carriage>,CarriageRepository>();
builder.Services.AddScoped<IRepository<TripStation>,TripStationRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
