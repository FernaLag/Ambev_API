namespace Ambev.Ominia.Application.Features.Users.Queries.GetUser;

/// <summary>
/// Command for retrieving a user by their ID
/// </summary>
public record GetUserQuery : IRequest<UserDto>
{
    /// <summary>
    /// The Id of the user to retrieve
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Initializes a new instance of GetUserCommand
    /// </summary>
    /// <param name="id">The ID of the user to retrieve</param>
    public GetUserQuery(int id)
    {
        Id = id;
    }
}