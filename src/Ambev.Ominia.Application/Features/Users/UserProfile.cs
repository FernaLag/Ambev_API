using Ambev.Ominia.Application.Features.Users.Commands.CreateUser;
using Ambev.Ominia.Domain.Entities.Users;

namespace Ambev.Ominia.Application.Features.Users;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, UserSummaryDto>();

        CreateMap<CreateUserCommand, User>();
        CreateMap<User, UserDto>();
    }
}