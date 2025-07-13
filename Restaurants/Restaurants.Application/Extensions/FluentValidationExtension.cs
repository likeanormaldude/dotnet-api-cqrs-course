using FluentValidation;
using FluentValidation.Results;
using Restaurants.Application.Models;

namespace Restaurants.Application.Extensions;

public static class FluentValidationExtension
{
    #region GroupedValidationFailure extensions
    public static IEnumerable<GroupedValidationFailure> ToGroupedValidationErrors(
        this IEnumerable<ValidationFailure> errors
    ) => errors.GroupBy(e => e.PropertyName).Select(ToGroupedValidationError);

    public static GroupedValidationFailure ToGroupedValidationError(
        IGrouping<string, ValidationFailure> groupedValidationFailure
    )
    {
        return new GroupedValidationFailure
        {
            PropertyName = groupedValidationFailure.Key,
            ErrorMessages = groupedValidationFailure.Select(x => x.ErrorMessage).ToList(),
        };
    }

    public static List<string> MapOnlyErrorMessages(
        this IEnumerable<GroupedValidationFailure> groupedValidationFailures
    ) => groupedValidationFailures.SelectMany(x => x.ErrorMessages).ToList();
    #endregion

    #region ValidationException extensions

    public static IEnumerable<GroupedValidationFailure> ToGroupedValidationErrors(
        this ValidationException validationException
    ) => validationException.Errors.GroupBy(e => e.PropertyName).Select(g => ToGroupedValidationError(g!));
    #endregion
}
