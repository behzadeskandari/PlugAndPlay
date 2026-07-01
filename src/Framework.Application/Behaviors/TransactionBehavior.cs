using Framework.Application.Requests;
using Framework.Application.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Application.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(IServiceScopeFactory scopeFactory, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ITransactionRequest)
        {
            return await next();
        }

        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
        if (unitOfWork is null)
        {
            return await next();
        }

        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            var response = await next();
            await unitOfWork.CommitAsync(cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction failed for {RequestType}", typeof(TRequest).Name);
            await unitOfWork.RollbackAsync(cancellationToken);
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
