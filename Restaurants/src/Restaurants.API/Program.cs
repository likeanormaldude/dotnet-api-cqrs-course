using Microsoft.EntityFrameworkCore;
using Restaurants.API.Extensions;
using Restaurants.API.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Infrastructure.Services;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddPresentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    using (IServiceScope scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantsDbContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
        dbContext.Database.Migrate(); // (optional, auto apply migrations)
        await seeder.Seed();

        var runner = new DataScriptRunner(dbContext, scope.ServiceProvider);
        await runner.RunAllAsync();
    }

    // Configure the HTTP request pipeline.
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseSerilogRequestLogging();

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeLoggingMiddleware>();
    app.UseMiddleware<ValidationExceptionMiddleware>();

    app.MapControllers();
    app.MapGroup("api/identity").WithTags("Identity").MapIdentityApi<User>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed with exception: {Exception}", ex);
    Console.WriteLine(ex.ToString());
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
