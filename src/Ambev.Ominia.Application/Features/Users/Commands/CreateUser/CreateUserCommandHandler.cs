using Ambev.Ominia.Domain.Entities.Users;

namespace Ambev.Ominia.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<CreateUserCommand, UserDto>
{

    public async Task<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
    var existingUser = await userRepository.GetByEmailAsync(command.Email, cancellationToken);
    if (existingUser != null)
        throw new InvalidOperationException($"User with email {command.Email} already exists");

    var user = mapper.Map<User>(command);

    var createdUser = await userRepository.CreateAsync(user, cancellationToken);
    return mapper.Map<UserDto>(createdUser);
    }
}