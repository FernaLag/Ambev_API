using Ambev.Ominia.Domain.Entities.Users;

namespace Ambev.Ominia.Domain.Events.Users;

public class UserRegisteredEvent(User user)
    {
    public User User { get; } = user;
    }