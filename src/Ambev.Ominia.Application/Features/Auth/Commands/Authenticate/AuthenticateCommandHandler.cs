using Ambev.Ominia.Crosscutting.Security;
using Ambev.Ominia.Domain.Specifications;

namespace Ambev.Ominia.Application.Features.Auth.Commands.Authenticate;

public class AuthenticateCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<AuthenticateCommand, AuthDto>
{
    public async Task<AuthDto> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null || !passwordHasher.VerifyPassword(request.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var activeUserSpec = new ActiveUserSpecification();
        if (!activeUserSpec.IsSatisfiedBy(user))
        {
            throw new UnauthorizedAccessException("User is not active");
        }

        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthDto(
            user.Id,
            user.Username,
            user.Email,
            user.Phone,
            user.Role.ToString(),
            token
        );
    }
}