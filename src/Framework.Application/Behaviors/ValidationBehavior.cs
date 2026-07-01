using FluentValidation;
using FluentResults;
using MediatR;

namespace Framework.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            if (failures.Any())
            {
                var messages = failures.Select(f => f.ErrorMessage).Distinct().ToArray();

                var responseType = typeof(TResponse);
                // If TResponse is Result or Result<T>
                if (responseType == typeof(Result))
                {
                    return (TResponse)(object)Result.Fail(string.Join("; ", messages));
                }

                if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var genericArg = responseType.GetGenericArguments()[0];
                    var failMethod = typeof(Result).GetMethod("Fail", new[] { typeof(string) })?.MakeGenericMethod(genericArg);
                    if (failMethod is not null)
                    {
                        var failed = failMethod.Invoke(null, new object[] { string.Join("; ", messages) })!;
                        return (TResponse)failed;
                    }
                }

                // Cannot construct proper Result<T>, throw validation exception as last resort
                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}
