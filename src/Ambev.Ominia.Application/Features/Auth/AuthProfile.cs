using Ambev.Ominia.Domain.Entities.Users;

namespace Ambev.Ominia.Application.Features.Auth;

public sealed class AuthProfile : Profile
{
    public AuthProfile()
    {
        // User to AuthenticationDto mapping
        CreateMap<User, AuthDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
    }
}