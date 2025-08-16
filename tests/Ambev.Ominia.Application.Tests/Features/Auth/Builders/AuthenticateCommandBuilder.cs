using Ambev.Ominia.Application.Features.Auth.Commands.Authenticate;
using Bogus;

namespace Ambev.Ominia.Application.Tests.Features.Auth.Builders;

public class AuthenticateCommandBuilder
{
    private string _email = string.Empty;
    private string _password = string.Empty;
    private readonly Faker _faker = new();

    public AuthenticateCommandBuilder()
    {
        WithDefaults();
    }

    private AuthenticateCommandBuilder WithDefaults()
    {
        _email = _faker.Internet.Email();
        _password = _faker.Internet.Password(8);
        return this;
    }

    public AuthenticateCommandBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public AuthenticateCommandBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public AuthenticateCommandBuilder WithEmptyEmail()
    {
        _email = string.Empty;
        return this;
    }

    public AuthenticateCommandBuilder WithEmptyPassword()
    {
        _password = string.Empty;
        return this;
    }

    public AuthenticateCommandBuilder WithInvalidEmail()
    {
        _email = "invalid-email";
        return this;
    }

    public AuthenticateCommandBuilder WithShortPassword()
    {
        _password = "123";
        return this;
    }

    public AuthenticateCommand Build()
    {
        return new AuthenticateCommand
        {
            Email = _email,
            Password = _password
        };
    }
}