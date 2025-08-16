using Ambev.Ominia.Application.Features.Auth.Commands.Authenticate;
using Ambev.Ominia.Application.Tests.Features.Auth.Builders;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Auth.Commands.Authenticate;

public class AuthenticateCommandValidatorTests
{
    private readonly AuthenticateValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new AuthenticateCommandBuilder()
            .WithEmail("test@example.com")
            .WithPassword("password123")
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_InvalidEmail_ShouldHaveValidationError(string email)
    {
        // Arrange
        var command = new AuthenticateCommandBuilder()
            .WithEmail(email)
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_InvalidEmailFormat_ShouldHaveValidationError()
    {
        // Arrange
        var command = new AuthenticateCommandBuilder()
            .WithInvalidEmail()
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_InvalidPassword_ShouldHaveValidationError(string password)
    {
        // Arrange
        var command = new AuthenticateCommandBuilder()
            .WithPassword(password)
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_ShortPassword_ShouldHaveValidationError()
    {
        // Arrange
        var command = new AuthenticateCommandBuilder()
            .WithShortPassword()
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("abc")]
    [InlineData("abcd")]
    [InlineData("abcde")]
    public void Validate_PasswordTooShort_ShouldHaveValidationError(string password)
    {
        // Arrange
        var command = new AuthenticateCommandBuilder()
            .WithPassword(password)
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("123456")]
    [InlineData("password123")]
    [InlineData("verylongpassword")]
    public void Validate_ValidPasswordLength_ShouldNotHaveValidationError(string password)
    {
        // Arrange
        var command = new AuthenticateCommandBuilder()
            .WithEmail("test@example.com")
            .WithPassword(password)
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}