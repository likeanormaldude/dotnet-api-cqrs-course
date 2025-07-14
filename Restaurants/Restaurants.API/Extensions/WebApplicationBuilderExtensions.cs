using MediatR;
using Microsoft.OpenApi.Models;
using Restaurants.API.Behaviors;
using Restaurants.API.Middlewares;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;
using Serilog;

namespace Restaurants.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    // Add services to the container.
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(GetAllRestaurantsQueryHandler).Assembly);
            cfg.LicenseKey = builder.Configuration["MediatR:LicenseKey"];
        });

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(cfg =>
        {
            string securityDefinitionId = "bearerAuth";

            cfg.AddSecurityDefinition(
                securityDefinitionId,
                new OpenApiSecurityScheme() { Type = SecuritySchemeType.Http, Scheme = "Bearer" }
            );

            cfg.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = securityDefinitionId,
                            },
                        },
                        []
                    },
                }
            );
        });

        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();
        builder.Services.AddScoped<ValidationExceptionMiddleware>();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<RestaurantsDbContext>();
        builder.Host.UseSerilog((context, cfg) => cfg.ReadFrom.Configuration(context.Configuration));
    }
}
