using Swashbuckle.AspNetCore.Filters;
using Ambev.Ominia.Application.Features.Auth.Commands.Authenticate;

namespace Ambev.Ominia.Api.SwaggerExamples;

public class AuthenticateCommandExample : IExamplesProvider<AuthenticateCommand>
{
    public AuthenticateCommand GetExamples()
    {
        return new AuthenticateCommand
        {
            Email = "admin@ambev.com",
            Password = "Admin123!"
        };
    }
}