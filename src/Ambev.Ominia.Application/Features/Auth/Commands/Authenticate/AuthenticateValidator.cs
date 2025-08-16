namespace Ambev.Ominia.Application.Features.Auth.Commands.Authenticate;

public class AuthenticateValidator : AbstractValidator<AuthenticateCommand>
{
    public AuthenticateValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}