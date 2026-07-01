namespace Framework.Application.Requests;

public interface ICacheableRequest
{
    string CacheKey { get; }
    TimeSpan? AbsoluteExpirationRelativeToNow { get; }
    TimeSpan? SlidingExpiration { get; }
}
