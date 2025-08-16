using Ambev.Ominia.Domain.Enums;
using Ambev.Ominia.Domain.ValueObjects;

namespace Ambev.Ominia.Application.Features.Users;

public record UserDto(
    int Id,
    string Username,
    string Email,
    string Phone,
    UserStatus Status,
    UserRole Role,
    Address? Address,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record UserSummaryDto(
    int Id,
    string Username,
    string Email,
    UserStatus Status,
    UserRole Role
);