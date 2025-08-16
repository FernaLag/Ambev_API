using Ambev.Ominia.Application.Common.Validators;
using Ambev.Ominia.Domain.Enums;

namespace Ambev.Ominia.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Validator for CreateUserCommand that defines validation rules for user creation command.
/// </summary>
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
    public CreateUserCommandValidator()
        {
        RuleFor(user => user.Email)
            .Email();

        RuleFor(user => user.Username)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(user => user.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(250);

        RuleFor(user => user.Phone)
            .Matches(@"^\+?[1-9]\d{1,14}$");

        RuleFor(user => user.Status)
            .NotEqual(UserStatus.Unknown);

        RuleFor(user => user.Role)
            .NotEqual(UserRole.None);
        }
    }