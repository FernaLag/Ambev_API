using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Ambev.Ominia.Crosscutting.Security;

/// <summary>
/// Custom authorization attribute that validates JWT tokens.
/// Provides a clean, reusable way to protect endpoints.
/// </summary>
public class RequireAuthAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Skip authorization if the action has [AllowAnonymous]
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }

        var token = ExtractTokenFromRequest(context.HttpContext.Request);

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Token is required" });
            return;
        }

        if (!ValidateToken(token, context.HttpContext))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Invalid or expired token" });
            return;
        }
    }

    private static string? ExtractTokenFromRequest(Microsoft.AspNetCore.Http.HttpRequest request)
    {
        var authHeader = request.Headers.Authorization.FirstOrDefault();

        if (authHeader != null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authHeader["Bearer ".Length..].Trim();
        }

        return null;
    }

    private static bool ValidateToken(string token, Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        try
        {
            var configuration = httpContext.RequestServices.GetRequiredService<IConfiguration>();
            var secretKey = configuration["Jwt:SecretKey"];

            if (string.IsNullOrEmpty(secretKey))
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            // Set the user context for the request
            httpContext.User = principal;

            return true;
        }
        catch (SecurityTokenExpiredException)
        {
            return false;
        }
        catch (SecurityTokenException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}