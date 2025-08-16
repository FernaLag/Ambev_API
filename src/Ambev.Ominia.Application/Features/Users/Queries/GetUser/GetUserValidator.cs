namespace Ambev.Ominia.Application.Features.Users.Queries.GetUser;

/// <summary>
/// Validator for GetUserCommand
/// </summary>
public class GetUserValidator : AbstractValidator<GetUserQuery>
{
    /// <summary>
    /// Initializes validation rules for GetUserCommand
    /// </summary>
    public GetUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}
