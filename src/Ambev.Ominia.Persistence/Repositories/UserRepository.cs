using Ambev.Ominia.Domain.Entities.Users;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Ambev.Ominia.Persistence.Repositories;

/// <summary>
/// Implementation of IUserRepository using Entity Framework Core
/// </summary>
/// <remarks>
/// Initializes a new instance of UserRepository
/// </remarks>
/// <param name="context">The database context</param>
public class UserRepository(PostgreeContext context) : IUserRepository
    {

    /// <summary>
    /// Creates a new user in the database
    /// </summary>
    /// <param name="user">The user to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user</returns>
    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return user;
        }

    /// <summary>
    /// Retrieves a user by their Id
    /// </summary>
    /// <param name="id">The Id of the user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
        return await context.Users.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

    /// <summary>
    /// Retrieves a user by their email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

    /// <summary>
    /// Deletes a user from the database
    /// </summary>
    /// <param name="id">The Id of the user to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the user was deleted, false if not found</returns>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
        var user = await GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"User with ID {id} not found");

        context.Users.Remove(user);

        await context.SaveChangesAsync(cancellationToken);
        }
    }