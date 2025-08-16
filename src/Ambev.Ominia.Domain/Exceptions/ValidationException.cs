using System.Runtime.Serialization;
using Ambev.Ominia.Domain.Common.Validations;

namespace Ambev.Ominia.Domain.Exceptions;

[Serializable]
[KnownType(typeof(List<ValidationFailure>))]
public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation failures occurred.")
    {
        Failures = [];
    }

    public ValidationException(ValidationFailure failure) : this()
    {
        Failures.Add(failure);
    }

    public ValidationException(string property, string message) : this()
    {
        Failures.Add(new ValidationFailure(property, message));
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Failures = [.. failures];
    }

    public ValidationException(IEnumerable<ValidationFailure> failures, string message) : base(message)
    {
        Failures = [.. failures];
    }

    public List<ValidationFailure> Failures { get; }

#if DEBUG
    public override string Message =>
        base.Message +
        Environment.NewLine +
        Environment.NewLine +
        string.Join(Environment.NewLine, Failures.Select(x => x.ToString()).ToList()) +
        Environment.NewLine;
#endif

    public override string ToString() => Message;
}