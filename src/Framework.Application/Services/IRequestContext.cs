namespace Framework.Application.Services;

public interface IRequestContext
{
    string? CorrelationId { get; }
    System.Security.Claims.ClaimsPrincipal? User { get; }
}
