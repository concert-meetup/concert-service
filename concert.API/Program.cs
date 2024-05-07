using concert.API.Data;
using concert.API.Data.Abstractions;
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

// var connectionString = builder.Configuration.GetConnectionString("LocalConnection");
var connectionString = builder.Configuration.GetConnectionString("DockerConnection");
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

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173"));

app.Run();
