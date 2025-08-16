namespace Ambev.Ominia.Domain.Entities.Users;

public interface IUser
{
    /// <summary>
    /// Obtém o identificador único do usuário.
    /// </summary>
    /// <returns>O ID do usuário como uma string.</returns>
    public string Id { get; }

    /// <summary>
    /// Obtém o nome de usuário.
    /// </summary>
    /// <returns>O nome de usuário.</returns>
    public string Username { get; }

    /// <summary>
    /// Obtém o papel/função do usuário no sistema.
    /// </summary>
    /// <returns>O papel do usuário como uma string.</returns>
    public string Role { get; }
}