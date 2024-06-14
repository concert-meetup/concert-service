using concert.API.Data;
using concert.API.IntegrationEvents;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.RabbitMq;

namespace concert.IntegrationTest.Config;

public class ConcertServiceWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ConcertDbContext>));
        
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        
            services.AddDbContext<ConcertDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });
        
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ConcertDbContext>();
        
            db.Database.EnsureCreated();
        
            try
            {
                ConcertDbContextDataSeed.SeedTestData(db);
            }
            catch (Exception ex)
            {
                var logger = scopedServices
                    .GetRequiredService<ILogger<ConcertServiceWebAppFactory>>();
        
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test data. Error: {Message}", ex.Message);
            }
        });
    }
}