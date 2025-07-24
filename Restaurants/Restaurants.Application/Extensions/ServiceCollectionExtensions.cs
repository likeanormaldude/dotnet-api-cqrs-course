using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Common;
using Restaurants.Application.Users;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    private static Type ApplicationType = typeof(ServiceCollectionExtensions);

    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMapper, ServiceMapper>();
        services.AddValidatorsFromAssemblyContaining(ApplicationType);
        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();
        services.AddSingleton(MapsterApplicationHelper.RegisterAll(new TypeAdapterConfig(), ApplicationType.Assembly));
    }
}
