using Ambev.Ominia.Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ambev.Ominia.Crosscutting.Security;

/// <summary>
/// Implementation of JWT (JSON Web Token) generator.
/// </summary>
/// <remarks>
/// Initializes a new instance of the JWT token generator.
/// </remarks>
/// <param name="configuration">Application configuration containing the necessary keys for token generation.</param>
public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{

    /// <summary>
    /// Generates a JWT token for a specific user.
    /// </summary>
    /// <param name="user">User for whom the token will be generated.</param>
    /// <returns>Valid JWT token as string.</returns>
    /// <remarks>
    /// The generated token includes the following claims:
    /// - NameIdentifier (User ID)
    /// - Name (Username)
    /// - Role (User role)
    ///
    /// The token is valid for 8 hours from the moment of generation.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when user or secret key is not provided.</exception>
    public string GenerateToken(IUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var secretKey = configuration["Jwt:SecretKey"]?.ToString();
        ArgumentException.ThrowIfNullOrWhiteSpace(secretKey);

        var keyBytes = Encoding.ASCII.GetBytes(secretKey);

        var claims = new[]
        {
           new Claim(ClaimTypes.NameIdentifier, user.Id),
           new Claim(ClaimTypes.Name, user.Username),
           new Claim(ClaimTypes.Role, user.Role)
       };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}