namespace Restaurants.Domain.Extensions;

public static class ListExtensions
{
    public static bool IsEmptyList<T>(this IEnumerable<T> list)
    {
        bool isEmpty = true;

        if (list == null || !list.Any())
            return isEmpty;

        return false;
    }
}
