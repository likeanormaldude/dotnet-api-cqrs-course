namespace Restaurants.Application.Users;

public record CurrentUser(string Id, string Email, IEnumerable<string> roles)
{
    public bool IsInRole(string role) => roles.Contains(role, StringComparer.OrdinalIgnoreCase);
}
