using FluentValidation;
using MediatR;

namespace Ambev.Ominia.Crosscutting.Behaviors;

/// <summary>
/// Pipeline behavior that automatically validates requests using FluentValidation before they reach the handler.
/// </summary>
/// <typeparam name="TRequest">The type of the request being validated.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
/// </remarks>
/// <param name="validators">A collection of FluentValidation validators for the request.</param>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{

    /// <summary>
    /// Handles the request validation before it reaches the request handler.
    /// </summary>
    /// <param name="request">The request object to be validated.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The response from the next handler in the pipeline.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(validators
                .Select(validator => validator.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var customFailures = failures.Select(f => new Domain.Common.Validations.ValidationFailure(f.PropertyName, f.ErrorMessage));
                throw new Ambev.Ominia.Domain.Exceptions.ValidationException(customFailures);
            }
        }

        return await next();
    }
}