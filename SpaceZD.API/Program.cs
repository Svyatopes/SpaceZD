using Microsoft.AspNetCore.Authentication.JwtBearer;
using SpaceZD.API.Extensions;
using SpaceZD.API.Middleware;
using SpaceZD.BusinessLayer.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithOptions();

builder.Services.AddAuthenticationExtension(JwtBearerDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

builder.AddDbContextScoped();
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(BusinessLayerMapper).Assembly);
builder.Services.AddRepositoriesScoped();
builder.Services.AddServicesScoped();


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