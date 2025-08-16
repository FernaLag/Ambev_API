using Ambev.Ominia.Application.Features.Auth.Commands.Authenticate;
using Ambev.Ominia.Application.Tests.Features.Auth.Builders;
using Ambev.Ominia.Crosscutting.Security;
using Ambev.Ominia.Domain.Entities.Users;
using Ambev.Ominia.Domain.Enums;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Auth.Commands.Authenticate;

public class AuthenticateCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthenticateCommandHandler _handler;

    public AuthenticateCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new AuthenticateCommandHandler(_userRepository, _passwordHasher, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ShouldReturnAuthDto()
    {
        // Arrange
        var command = new AuthenticateCommandBuilder()
            .WithEmail("test@example.com")
            .WithPassword("password123")
            .Build();
        
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            Phone = "123456789",
            Password = "hashedpassword",
            Role = UserRole.Customer,
            Status = UserStatus.Active
        };
        
        var expectedToken = "jwt-token";
        
        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(true);
        _jwtTokenGenerator.GenerateToken(user).Returns(expectedToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(user.Id);
        result.Name.ShouldBe(user.Username);
        result.Email.ShouldBe(user.Email);
        result.Phone.ShouldBe(user.Phone);
        result.Role.ShouldBe(user.Role.ToString());
        result.Token.ShouldBe(expectedToken);
        
        await _userRepository.Received(1).GetByEmailAsync(command.Email, Arg.Any<CancellationToken>());
        _passwordHasher.Received(1).VerifyPassword(command.Password, user.Password);
        _jwtTokenGenerator.Received(1).GenerateToken(user);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var command = new AuthenticateCommandBuilder().Build();
        
        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((User?)null);

        // Act & Assert
        var exception = await Should.ThrowAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        exception.Message.ShouldBe("Invalid credentials");
        await _userRepository.Received(1).GetByEmailAsync(command.Email, Arg.Any<CancellationToken>());
        _passwordHasher.DidNotReceive().VerifyPassword(Arg.Any<string>(), Arg.Any<string>());
        _jwtTokenGenerator.DidNotReceive().GenerateToken(Arg.Any<User>());
    }

    [Fact]
    public async Task Handle_InvalidPassword_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var command = new AuthenticateCommandBuilder().Build();
        
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = command.Email,
            Phone = "123456789",
            Password = "hashedpassword",
            Role = UserRole.Customer,
            Status = UserStatus.Active
        };
        
        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(false);

        // Act & Assert
        var exception = await Should.ThrowAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        exception.Message.ShouldBe("Invalid credentials");
        await _userRepository.Received(1).GetByEmailAsync(command.Email, Arg.Any<CancellationToken>());
        _passwordHasher.Received(1).VerifyPassword(command.Password, user.Password);
        _jwtTokenGenerator.DidNotReceive().GenerateToken(Arg.Any<User>());
    }

    [Fact]
    public async Task Handle_InactiveUser_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var command = new AuthenticateCommandBuilder().Build();
        
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = command.Email,
            Phone = "123456789",
            Password = "hashedpassword",
            Role = UserRole.Customer,
            Status = UserStatus.Inactive
        };
        
        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(true);

        // Act & Assert
        var exception = await Should.ThrowAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        exception.Message.ShouldBe("User is not active");
        await _userRepository.Received(1).GetByEmailAsync(command.Email, Arg.Any<CancellationToken>());
        _passwordHasher.Received(1).VerifyPassword(command.Password, user.Password);
        _jwtTokenGenerator.DidNotReceive().GenerateToken(Arg.Any<User>());
    }

    [Fact]
    public async Task Handle_SuspendedUser_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var command = new AuthenticateCommandBuilder().Build();
        
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = command.Email,
            Phone = "123456789",
            Password = "hashedpassword",
            Role = UserRole.Customer,
            Status = UserStatus.Suspended
        };
        
        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(true);

        // Act & Assert
        var exception = await Should.ThrowAsync<UnauthorizedAccessException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        exception.Message.ShouldBe("User is not active");
        await _userRepository.Received(1).GetByEmailAsync(command.Email, Arg.Any<CancellationToken>());
        _passwordHasher.Received(1).VerifyPassword(command.Password, user.Password);
        _jwtTokenGenerator.DidNotReceive().GenerateToken(Arg.Any<User>());
    }
}