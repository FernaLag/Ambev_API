using Swashbuckle.AspNetCore.Filters;
using Ambev.Ominia.Application.Features.Users.Commands.CreateUser;
using Ambev.Ominia.Domain.Enums;

namespace Ambev.Ominia.Api.SwaggerExamples;

public class CreateUserCommandExample : IExamplesProvider<CreateUserCommand>
{
    public CreateUserCommand GetExamples()
    {
        return new CreateUserCommand
        {
            Username = "fernalag",
            Email = "fernalag@example.com",
            Phone = "+5511999999999",
            Password = "ferna123!",
            Role = UserRole.Customer,
            Status = UserStatus.Active
        };
    }
}