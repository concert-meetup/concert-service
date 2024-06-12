using concert.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace concert.IntegrationTest;

public class ConcertServiceWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ConcertDbContext>));

            var connectionString = GetConnectionString();
            services.AddDbContext<ConcertDbContext>(o =>
            {
                o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureDeleted();
        });
    }

    private static string? GetConnectionString()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<ConcertServiceWebAppFactory>()
            .Build();

        var connectionString = config.GetConnectionString("ConcertService");
        return connectionString;
    }

    private static ConcertDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ConcertDbContext>();
        return dbContext;
    }
}