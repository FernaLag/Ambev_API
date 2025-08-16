using Ambev.Ominia.Domain.Extensions;
using FluentValidation.Validators;

namespace Ambev.Ominia.Application.Common.Validators;

public class EmailValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "EmailValidator";

    public override bool IsValid(ValidationContext<T> context, string value) => value.IsValidEmail();

    protected override string GetDefaultMessageTemplate(string errorCode) => "E-mail inválido.";
}