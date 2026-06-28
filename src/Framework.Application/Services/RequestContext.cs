using Microsoft.AspNetCore.Http;

namespace Framework.Application.Services;

public sealed class RequestContext : IRequestContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? CorrelationId => _httpContextAccessor.HttpContext?.TraceIdentifier;

    public System.Security.Claims.ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
}
