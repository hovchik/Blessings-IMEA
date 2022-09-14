using FluentValidation;
using MediatR;

namespace Blessings.Order.Core.BehaviourPipelines;

public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly List<IValidator<TRequest>> _validators;

    public ValidationPipeline(List<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_validators.Any())
        {
            var validationContext = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(validationContext, cancellationToken)));
            var failures = validationResults.SelectMany(validationResult => validationResult.Errors).Where(validationFailure => validationFailure != null).ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);
        }
        return await next();
    }
}
