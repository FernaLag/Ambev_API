using Ambev.Ominia.Domain.Entities.Users;

namespace Ambev.Ominia.Crosscutting.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(IUser user);
}