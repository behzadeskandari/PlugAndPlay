using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Framework.Application.Behaviors;

public sealed class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for {RequestType}", typeof(TRequest).Name);
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var genericType = typeof(TResponse).GetGenericArguments()[0];
                var failure = typeof(Result).GetMethod(nameof(Result.Fail), new[] { typeof(string) })?.MakeGenericMethod(genericType);
                var response = failure?.Invoke(null, new object[] { ex.Message });
                return (TResponse)response!;
            }

            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Fail(ex.Message);
            }

            throw;
        }
    }
}
