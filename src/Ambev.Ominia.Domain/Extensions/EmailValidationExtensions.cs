using System.Text.RegularExpressions;

namespace Ambev.Ominia.Domain.Extensions;

public static class EmailValidationExtensions
    {
    private static readonly Regex Regex = new(
        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.IgnoreCase,
        TimeSpan.FromSeconds(2)
    );

    public static bool IsValidEmail(this string email)
        {
        return !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email);
        }
    }