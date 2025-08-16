namespace Ambev.Ominia.Application.Features.Auth;

public record AuthDto(
    int Id,
    string Name,
    string Email,
    string Phone,
    string Role,
    string Token
);