using concert.API.Data;
using concert.API.Data.Abstractions;
using concert.API.IntegrationEvents;
using concert.API.Service;
using concert.API.Service.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IConcertService, ConcertService>();

builder.Services.AddScoped<IConcertRepository, ConcertRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();

builder.Services.AddScoped<IEventBus, EventBus>();
// builder.Services.AddScoped<IEventBus, RabbitMQTest>();

// builder.Services.AddHostedService<RabbitMQTest>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                       $"User Id={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

builder.Services.AddDbContext<ConcertDbContext>(o =>
    o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:8000"));
// app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://kind-sea-0cfd9c303.5.azurestaticapps.net"));
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();
