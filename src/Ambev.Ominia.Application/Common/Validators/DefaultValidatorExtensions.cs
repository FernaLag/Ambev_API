namespace Ambev.Ominia.Application.Common.Validators;

public static class DefaultValidatorExtensions
    {
    public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
        return ruleBuilder.SetValidator(new EmailValidator<T>());
        }
    }