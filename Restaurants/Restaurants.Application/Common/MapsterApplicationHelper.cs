using System.Reflection;
using Mapster;

namespace Restaurants.Application.Common;

/// <summary>
/// By adding this helper as a SingleTon service in the DI container, "/mappings" will be automatically registered in the API project.
/// </summary>
public static class MapsterApplicationHelper
{
    private static Type ApplicationType = typeof(MapsterApplicationHelper);

    /// <summary>
    /// Registers all Mapster configurations from the specified assembly.
    /// </summary>
    public static TypeAdapterConfig RegisterAll(TypeAdapterConfig? config = null, Assembly? assembly = null)
    {
        if (config == null)
        {
            config = new TypeAdapterConfig();
        }

        if (assembly == null)
        {
            assembly = ApplicationType.Assembly;
        }

        IEnumerable<Type> mappingTypes = assembly
            .GetTypes()
            .Where(t => typeof(IRegisterMapsterConfig).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (Type type in mappingTypes)
        {
            ((IRegisterMapsterConfig)Activator.CreateInstance(type)!).Register(config);
        }

        return config;
    }
}
