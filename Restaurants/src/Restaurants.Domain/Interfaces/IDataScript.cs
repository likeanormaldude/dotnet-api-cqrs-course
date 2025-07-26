using Microsoft.EntityFrameworkCore;

namespace Restaurants.Domain.Interfaces;

public interface IDataScript<in TContext>
    where TContext : DbContext
{
    string Name { get; }
    Task RunAsync(TContext context, IServiceProvider services);
}
