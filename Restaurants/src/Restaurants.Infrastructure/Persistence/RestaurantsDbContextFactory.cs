using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Restaurants.Infrastructure.Persistence;

public class RestaurantsDbContextFactory : IDesignTimeDbContextFactory<RestaurantsDbContext>
{
    public RestaurantsDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Restaurants.API"));

        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<RestaurantsDbContext>();
        string connectionString = config.GetConnectionString("RestaurantsDb") ?? "";
        optionsBuilder.UseSqlServer(connectionString);

        return new RestaurantsDbContext(optionsBuilder.Options);
    }
}
