using System.Collections.Concurrent;
using Framework.Application.Requests;
using MediatR;

namespace Framework.Application.Behaviors;

public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private static readonly ConcurrentDictionary<string, object> Cache = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICacheableRequest cacheableRequest)
        {
            return await next();
        }

        var cacheKey = cacheableRequest.CacheKey;
        if (string.IsNullOrWhiteSpace(cacheKey))
        {
            return await next();
        }

        if (Cache.TryGetValue(cacheKey, out var cachedValue))
        {
            return (TResponse)cachedValue;
        }

        var response = await next();
        Cache.TryAdd(cacheKey, response!);
        return response;
    }
}
