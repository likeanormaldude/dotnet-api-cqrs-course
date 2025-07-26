using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotReturnValidationErrors()
    {
        #region Arrange
        CreateRestaurantCommand command = new()
        {
            Name = "Test",
            Description = "A great place to eat.",
            Category = "Italian",
            PostalCode = "H3Y 3F2",
            ContactEmail = "test@test.com",
        };

        CreateRestaurantCommandValidator validator = new();
        #endregion
        #region Act
        var validationResult = validator.TestValidate(command);
        #endregion
        #region Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
        #endregion
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldReturnValidationErrors()
    {
        #region Arrange
        CreateRestaurantCommand command = new()
        {
            Name = "Te",
            Description = "A great place to eat.",
            Category = "",
            PostalCode = "H3Y 3F2",
            ContactEmail = "test@",
        };

        CreateRestaurantCommandValidator validator = new();
        #endregion

        #region Act
        var validationResult = validator.TestValidate(command);
        #endregion

        #region Assert
        Console.WriteLine(validationResult.ToString());
        //validationResult.ShouldHaveValidationErrors();
        validationResult.ShouldHaveValidationErrorFor(dto => dto.Name);
        validationResult.ShouldHaveValidationErrorFor(dto => dto.Category);
        validationResult.ShouldHaveValidationErrorFor(dto => dto.ContactEmail);
        #endregion
    }
}
