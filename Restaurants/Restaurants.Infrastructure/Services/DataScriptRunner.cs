using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Services;

public class DataScriptRunner(RestaurantsDbContext dbContext, IServiceProvider services)
{
    private readonly RestaurantsDbContext _dbContext = dbContext;
    private readonly IServiceProvider _services = services;

    public async Task RunAllAsync()
    {
        var history = _dbContext.DataScriptHistories.Select(x => x.ScriptName).ToHashSet();
        Type dataScriptType = typeof(IDataScript<RestaurantsDbContext>);

        // Find all IDataScript implementations using reflection
        var scripts = typeof(DataScriptRunner)
            .Assembly.GetTypes()
            .Where(t => dataScriptType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IDataScript<RestaurantsDbContext>)Activator.CreateInstance(t)!)
            .OrderBy(s => s.Name);

        foreach (var script in scripts)
        {
            if (!history.Contains(script.Name))
            {
                await script.RunAsync(_dbContext, _services);
                _dbContext.Add(new DataScriptHistory { ScriptName = script.Name, RanAt = DateTime.UtcNow });
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
