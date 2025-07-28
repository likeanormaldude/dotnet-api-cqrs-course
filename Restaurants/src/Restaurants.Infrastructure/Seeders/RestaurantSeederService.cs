using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using System.Text.Json;

namespace Restaurants.Infrastructure.Seeders;

public static class RestaurantSeederService
{
    private const string PARENT_DIR = "Seeders";
    private const string DIR_DATA = "SeedData";
    private static JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

    public static IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant>? restaurants = ReadFromJsonSeedData<List<Restaurant>?>("restaurants-seed.json");
        return restaurants ?? new List<Restaurant>();
    }

    public static IEnumerable<IdentityRole> GetRoles()
    {
        IEnumerable<IdentityRole> roles =
        [
            new(UserRoles.User) { NormalizedName = UserRoles.User.ToUpperInvariant() },
            new(UserRoles.Owner) { NormalizedName = UserRoles.Owner.ToUpperInvariant() },
            new(UserRoles.Admin) { NormalizedName = UserRoles.Admin.ToUpperInvariant() },
        ];

        return roles;
    }

    private static T? ReadFromJsonSeedData<T>(string filename)
    {
        string json = string.Empty;

        try
        {
            // All possible locations, considering differences in project structure between local and Azure
            string[] possiblePaths =
            {
                Path.Combine(AppContext.BaseDirectory, DIR_DATA, filename),
                Path.Combine(AppContext.BaseDirectory, PARENT_DIR, DIR_DATA, filename),
            };

            string? pathFound = possiblePaths.FirstOrDefault(File.Exists);

            if (pathFound == null)
                throw new FileNotFoundException(
                    $"Could not find seed data file: {filename}. Tried: {string.Join(", ", possiblePaths)}"
                );

            json = File.ReadAllText(pathFound);
        }
        catch (FileNotFoundException ex)
        {
            throw new InvalidOperationException(
                $"Failed to read seed data from file '{filename}'. Ensure the file exists in the expected location.",
                ex
            );
        }

        T? contents = JsonSerializer.Deserialize<T>(json, options);
        return contents;
    }
}
