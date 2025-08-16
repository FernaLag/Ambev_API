namespace Ambev.Ominia.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
    {

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
        await userRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }