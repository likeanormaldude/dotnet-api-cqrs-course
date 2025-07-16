using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Seeders;

public static class RestaurantSeederService
{
    private const string DIR_DATA = "SeedData";
    private static JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

    public static IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant>? restaurants = ReadFromJsonSeedData<List<Restaurant>?>("restaurants-seed.json");
        return restaurants ?? new List<Restaurant>();
    }

    public static IEnumerable<IdentityRole> GetRoles()
    {
        IEnumerable<IdentityRole> roles = [new(UserRoles.User), new(UserRoles.Owner), new(UserRoles.Admin)];

        return roles;
    }

    private static T? ReadFromJsonSeedData<T>(string filename)
    {
        string path = Path.Combine(AppContext.BaseDirectory, DIR_DATA, filename);
        string json = File.ReadAllText(path);
        T? contents = JsonSerializer.Deserialize<T>(json, options);

        return contents;
    }
}
