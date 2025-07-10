namespace Restaurants.Application.Models;

public class GroupedValidationFailure
{
    public string PropertyName { get; set; } = default!;
    public List<string> ErrorMessages { get; set; } = [];
}
