namespace Ambev.Ominia.Application.Features.Users.Queries.GetUser;

public class GetUserQueryHandler(
    IUserRepository userRepository,
    IMapper mapper) : IRequestHandler<GetUserQuery, UserDto>
{

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        return user is null
            ? throw new KeyNotFoundException($"User with ID {request.Id} not found")
            : mapper.Map<UserDto>(user);
    }
}