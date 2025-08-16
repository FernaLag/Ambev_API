namespace Ambev.Ominia.Application.Features.Users.Commands.DeleteUser;

/// <summary>
/// Command for deleting a user
/// </summary>
public record DeleteUserCommand : IRequest
{
    /// <summary>
    /// The Id of the user to delete
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Initializes a new instance of DeleteUserCommand
    /// </summary>
    /// <param name="id">The ID of the user to delete</param>
    public DeleteUserCommand(int id)
    {
        Id = id;
    }
}