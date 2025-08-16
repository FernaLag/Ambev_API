using Ambev.Ominia.Domain.Entities.Users;
using Ambev.Ominia.Domain.Enums;
using Ambev.Ominia.Domain.Interfaces;

namespace Ambev.Ominia.Domain.Specifications;

public class ActiveUserSpecification : ISpecification<User>
{
    public bool IsSatisfiedBy(User user)
    {
        return user.Status == UserStatus.Active;
    }
}